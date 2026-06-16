# Plane-0 guide: iac (Terraform on Azure)

> **Spike artifact (task 2.1).** This is the `dotnet-iac` skill converted to a
> Plane-0 **knowledge guide** — data, not an engine. It contains *what* and *why*
> (objective, naming, decision rationale, artifacts, inline templates). It contains
> **no** "Steps" section and **no** self-repair / checkbox / success-summary logic:
> that behaviour belongs to the generic `at-apply` engine, not to the domain.
>
> The non-linear part of this domain (3 deploy targets; an interactive
> `terraform apply`) is what the spike validates the engine can carry via `at-plan`
> (choice → tasks) and the `confirm` task-type in `at-apply`.

## Objective

Provide a runnable Terraform configuration for a solution on Azure, with a deploy
target chosen from Container Apps, App Service, or AKS, add-on resources inferred
from the Aspire AppHost, and a stable outputs contract consumed by the cicd guide.

## Choice point (non-linear) — deploy target

`at-plan` MUST surface this as a single question with a recommended default; the
answer parametrises which `compute.tf` template is injected. This is the part a
per-domain skill used to "decide" and the engine cannot — so the guide states the
options and the plan asks.

| Target | When | `compute.tf` resources |
|--------|------|------------------------|
| `container-apps` (default) | fully-managed serverless | `azurerm_container_app_environment` + `azurerm_container_app` per service + Log Analytics |
| `app-service` | classic PaaS | `azurerm_service_plan` + `azurerm_linux_web_app` per service |
| `aks` | self-managed Kubernetes | `azurerm_kubernetes_cluster` + default node pool |

## Inputs (with defaults)

- `target` ∈ {container-apps, app-service, aks} — default `container-apps`.
- `location` — default `westeurope`.
- `env` suffix — default `dev`.
- `project_name` — inferred from `.slnx` (placeholder `__PROJECT_NAME__`).

## Add-on inference (non-linear) — scan AppHost

Provision **only** the add-ons declared in `src/__PROJECT_NAME__.AppHost/Program.cs`.
This is data the engine substitutes; the plan lists detected add-ons for the user.

| AppHost call | Provisioned resource |
|--------------|----------------------|
| `AddSqlServer` / `AddSqlServerDatabase` | `azurerm_mssql_server` + `azurerm_mssql_database` |
| `AddRedis` | `azurerm_redis_cache` |
| `AddPostgres` | `azurerm_postgresql_flexible_server` |
| `AddAzureServiceBus` | `azurerm_servicebus_namespace` |
| `AddAzureStorage` | `azurerm_storage_account` |

## Naming

- Folder: `terraform/` at repo root.
- RG: `rg-<project>-<env>` (lowercase).
- ACR: `acr<project><env><suffix>` (lowercase, alnum only — ACR rule, 5–50 chars).
- Key Vault: `kv-<project>-<env>-<suffix>` (random suffix → global uniqueness).
- Variables: `project_name`, `location`, `env`, `target`, `image_tag`.
- Outputs (contract): `resource_group_name`, `key_vault_uri`,
  `container_registry_login_server`, `app_target_resource_id`, `app_target_kind`.

## Minimum artifacts

| File | Purpose |
|------|---------|
| `main.tf` | Provider blocks and pinned versions. |
| `variables.tf` | Every input with `type` and `description`. |
| `outputs.tf` | Stable contract for downstream cicd guide. |
| `locals.tf` | Naming helpers. |
| `compute.tf` | Target-specific resources (per choice above). |
| `addons.tf` | Only the add-ons detected from AppHost. |
| `identity.tf` | User-assigned managed identity for the compute target. |
| `keyvault.tf` | Azure Key Vault + access policies. |
| `containerregistry.tf` | ACR for container-based targets. |
| `backend.tf` | Commented placeholder for remote state. |
| `terraform.tfvars.example` | Template to copy. |
| `.gitignore` | Excludes `*.tfvars`, `*.tfstate*`, `.terraform/`. |

## Decision rationale (the *why* — injected inline into tasks.md before apply)

- **Three targets:** customer base spans serverless / PaaS / Kubernetes; one guide,
  no fork.
- **Pin `azurerm ~> 4.10`:** provider majors have broken in the past; floor allows
  patch updates, blocks surprise majors.
- **Detect add-ons from AppHost:** AppHost is the canonical resource declaration;
  hand-redeclaring drifts.
- **Stable `outputs.tf` contract:** cicd consumes these outputs; pinning names/types
  lets both evolve independently — breaking it is a major bump for both.
- **User-assigned managed identity:** outlives the compute target → predictable
  blue/green, recreate, KV access. System-assigned re-grants on every recreate.
- **Secrets in Key Vault, not App Settings:** auditable, rotatable, RBAC-scoped.
- **`terraform init -backend=false` on day one:** the guide cannot create a state
  backend (no creds); validate locally, uncomment `backend.tf` once provisioned.

## Inline templates (substituted by `at-plan`, written by `at-apply`)

`main.tf` provider pin:

```hcl
terraform {
  required_version = ">= 1.7.0"
  required_providers {
    azurerm = { source = "hashicorp/azurerm", version = "~> 4.10" }
    random  = { source = "hashicorp/random",  version = "~> 3.6" }
  }
}
provider "azurerm" { features {} }
```

`outputs.tf` stable contract (MUST NOT change between minor versions):

```hcl
output "resource_group_name"            { value = azurerm_resource_group.this.name }
output "key_vault_uri"                  { value = azurerm_key_vault.this.vault_uri }
output "container_registry_login_server" {
  value       = try(azurerm_container_registry.this[0].login_server, null)
  description = "ACR login server (null when target=app-service)."
}
output "app_target_resource_id" {
  value       = local.app_target_resource_id
  description = "Resource ID of the deployable target."
}
output "app_target_kind" { value = var.target }
```

## Quality checklist (verification the engine runs, not domain logic)

- [ ] Provider versions pinned with `~>`.
- [ ] `terraform validate` succeeds without a backend.
- [ ] `outputs.tf` exposes the documented contract names/types.
- [ ] Only AppHost-declared add-ons provisioned.
- [ ] Secrets in Key Vault, referenced from compute target.
- [ ] User-assigned managed identity is the KV/ACR principal.
- [ ] `.gitignore` excludes state and real `*.tfvars`.
- [ ] `backend.tf` present but commented.

## What was dropped (was engine, now provided by `at-apply`)

These sections of the original `dotnet-iac` SKILL.md are **not** in this guide because
they are generic engine behaviour:

- Pre-flight CLI checks → ordinary `bash` verify-tasks emitted by `at-plan`.
- "Confirm Before Proceeding" prompt → the `confirm` task-type in `at-apply`.
- The numbered "Steps 1–10" sequence → `at-apply`'s parse→classify→execute→verify loop.
- Self-repair, mark `[x]`, success summary → `at-apply` core behaviour.
- ADR/runbook authoring → `at-plan` phase-end output (shared across all domains).
