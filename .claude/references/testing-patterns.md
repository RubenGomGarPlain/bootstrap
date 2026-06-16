# Testing Patterns Checklist

Extracted from engineering-conventions testing section.

## Unit Tests

- [ ] Isolated, fast, no I/O
- [ ] One logical assertion per test
- [ ] Arrange-Act-Assert pattern
- [ ] Test doubles: prefer fakes over mocks
- [ ] No test interdependencies (each test runs in isolation)

## Integration Tests

- [ ] Use WebApplicationFactory for API tests
- [ ] Test real database with containers (Testcontainers)
- [ ] Verify cross-layer behavior end-to-end
- [ ] Clean state between tests

## Test Naming & Organization

- [ ] Naming: Method_Scenario_ExpectedResult or Given_When_Then
- [ ] Tests colocated with feature (or mirror source structure)
- [ ] Shared fixtures for expensive setup
- [ ] Test categories/traits for selective execution

## Architecture Tests

- [ ] Validate module boundaries (no illegal references)
- [ ] Enforce naming conventions
- [ ] Verify dependency direction
- [ ] Run as part of unit test suite

## Acceptance Tests

- [ ] Cover critical user journeys
- [ ] Written in business language where possible
- [ ] Stable selectors (not brittle to UI changes)
- [ ] Run in CI on every PR

## CI Integration

- [ ] Tests run in CI (fail the build on failure)
- [ ] Coverage target: 80%+ on business logic
- [ ] Test results published as build artifacts
- [ ] Flaky tests tracked and fixed promptly
- [ ] Parallel execution where safe
