# Plane-0 guide: cicd (Azure DevOps pipeline)

> Knowledge from `dotnet-cicd`. Consumes the iac guide's stable `outputs.tf` contract.
> Engine behaviour stripped.

## Objective
Generate `azure-pipelines.yml` (+ `pipelines/templates/`) implementing the canonical stages
**Build → Infra → Package → Deploy**, consuming the Terraform outputs from the iac guide.

## Choice point (non-linear) — tier
| Tier | Adds |
|------|------|
| `basic` (default) | four canonical stages only |
| `advanced` | security scanning (SAST, dependency audit, container scan), quality gates, composable custom stages |

## Inputs (with defaults)
- tier — `basic` (default) or `advanced`.
- service connection — default `azure-prod`.
- ACR service connection — optional, default same as main.
- variable group — default `{Project}-secrets`.
- (advanced) SAST tool — `codeql` (default) | `sonarcloud` | `security-code-scan`.
- (advanced) coverage threshold — default `80%`.

## Contract dependency
Consumes iac outputs: `resource_group_name`, `key_vault_uri`,
`container_registry_login_server`, `app_target_resource_id`, `app_target_kind`. These names
are frozen — see iac guide.

## Decision rationale (inline into tasks.md)
- **Four canonical stages:** Build (compile/test), Infra (terraform apply), Package (container/artefact), Deploy — a fixed spine every project shares.
- **Outputs contract, not hand-wiring:** pipeline reads Terraform outputs → iac and cicd evolve independently.
- **Advanced tier opt-in:** security gates add cost/time; basic ships fast, advanced hardens.

## What was dropped (now `at-apply`)
Pre-flight → bash; YAML/template writes + Steps → engine; gate thresholds are data in the YAML, not engine logic.
