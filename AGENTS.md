# AGENTS.md

> This file is the single source of project knowledge and agent guidance — keep
> all of it in here and in the code/config, and keep it up to date. Do **not**
> rely on per-machine agent "memory" features; that state does not travel with
> the repository.

## Project

A small **3D viewer application** written in **C#**.

- **Solution:** `ATDD_CS.slnx` (.NET SDK-style projects, central package management)
- **Runtime:** .NET 10 (`net10.0`)
- **Test framework:** xUnit (v3)
- **GUI framework:** WPF — the `Viewer.Wpf` adapter (MVVM with a humble view
  model); the core never depends on it.
- **Target:** Windows, for both development and execution.

Keep the core free of UI-framework, OS-specific, and filesystem dependencies;
anything technology-specific lives in an adapter behind a port.

## Architecture

**Hexagonal (ports & adapters).** A pure, UI-free **core** sits at the center; the UI is an **adapter** plugged into ports the core defines. Dependencies point **inward** (`adapters → core`); the core depends on nothing external.

```
Viewer.Core/         ← pure C#, no UI framework; fully covered by xUnit
  Geometry/    Vec3, Mesh, Triangle, BoundingBox
  Analysis/    metrics (surface area, volume, …)
  App/         ViewerService (inbound port), camera & presentation logic
  Ports/       outbound — IView + IModelSource interfaces the adapters implement
  IO/          importers (OBJ, …) parsing in-memory text
Viewer.Core.Tests/   ← xUnit; drives inbound ports against fake outbound ports
Viewer.Wpf/          ← WPF adapter (MVVM): ViewerViewModel implements the View
                       port, XAML views bind to it, FileModelSource implements
                       IModelSource. Humble, except extracted conversion logic.
Viewer.Wpf.Tests/    ← xUnit; unit-tests ONLY the adapter's extracted conversion
                       functions (e.g. MeshGeometryBuilder), headless
```

**Two ports for the UI:**

- **Inbound (commands)** — the UI adapter calls these: `OpenModel(path)`, `Orbit(…)`, `Zoom(…)`, `Pick(x, y)`, `SelectMetric(…)`.
- **Outbound (view)** — the core pushes view-state out through an interface the UI implements: `ShowModel(…)`, `ShowModelInfo(…)`, `ShowCamera(…)`, `ShowError(…)`. Tests substitute a fake view and assert on it.

The adapter follows **MVVM, kept humble**: `ViewerViewModel` implements the View port. The core pushes state; the view model converts it once into bindable WPF values (`MeshGeometry3D`, `PerspectiveCamera`, formatted strings) and raises change notifications; the XAML views just bind. Inbound, the views call thin view-model methods (`OpenModel`/`Orbit`/`Zoom`) that forward to the core service. Code-behind holds only what must stay there: showing dialogs and translating raw input events into command parameters. Plain `INotifyPropertyChanged` — no MVVM framework dependency. The view model itself holds no logic: anything algorithmic (e.g. building the flat-shaded `MeshGeometry3D`) is extracted into a pure conversion function — the only adapter code that is unit-tested.

**Rules that keep the core testable:**

- **Port interfaces use only core/domain types** (our `Vec3`, plain `RenderData`/`CameraState` types, `string`) — **never UI-framework types.** The adapter translates framework ↔ core types at the boundary.
- **Decisions/transformations live in the core; tech mechanics live in the adapter.** Don't route trivial pass-throughs through ceremony, but camera math, picking, parsing, and presentation logic belong in the core (and are prime TDD targets). Only draw calls and raw event capture stay in the UI.
- **Parsing is core logic** that works on in-memory text (testable without touching the filesystem); reading bytes from disk is an adapter concern (`IModelSource`).
- **Culture-invariant numbers.** Model files use `.` as the decimal separator regardless of OS locale; all numeric parsing/formatting in the core uses `CultureInfo.InvariantCulture`.
- **Test logic, not plumbing.** The view model and code-behind are untested pass-through. When adapter code grows logic, extract it into a pure conversion function and unit-test that (`Viewer.Wpf.Tests`, headless) — or move it into the core when it is not WPF-specific. Adapter tests assert conversions only; they never re-drive scenarios the core tests already own.

## Development methodology: TDD

Use **Test-Driven Development throughout**, following strict **red-green-refactor**:

1. **Red** — Write the *minimal* failing test that expresses the next small piece of desired behavior.
2. **Green** — Write the *minimal* implementation needed to make that test pass. No more than required.
3. **Refactor** — Improve the design without changing behavior, keeping tests green.

**TDD scope:** the core, plus the adapter's extracted conversion functions. The rest of the UI adapter (XAML, view-model plumbing, code-behind) is deliberately built pragmatically, without TDD — UI mechanics are a poor fit for it, and it holds no logic worth driving by tests.

### Test design

Prefer **integrated, high-level tests** that exercise behavior through public interfaces over fine-grained unit tests. High-level tests stay stable as the implementation changes, so they survive refactoring instead of breaking on it. Write a unit test only where it gives a clear benefit and won't turn fragile over time — e.g. isolating tricky algorithmic or edge-case logic that is hard to drive from the outside.

**Test organization:** one test class per file, named after the behavior area it covers (e.g. `OpenModelTests` for loading, `CameraInteractionTests` for the camera commands). Shared fakes and fixtures live in `Viewer.Core.Tests/Support/`. New tests go in the class whose behavior area they belong to — or a new class when they start a new area.

### Working agreements for the agent

- **Do not run tests or the application yourself.** When a test or build is expected to be red or green, **ask the user** to run it and report whether it is red or green (or what the failure is).
- **During the refactor step**, propose your own refactoring ideas **and explicitly ask the user** for additional refactorings they would like to see before proceeding.
- **Commit after every step** — after each **red**, each **green**, and each **refactor** step. Use clear commit messages indicating which step it is (e.g. `red: ...`, `green: ...`, `refactor: ...`).
- **Explain before changing tooling/config/environment.** Before applying changes to git config, build tooling, or the environment, explain what each change does (including what specific settings mean) and why, then get the go-ahead. Prefer non-destructive, verifiable steps — clarity matters more than speed.

### Step checklist

- [ ] Write minimal failing test (**red**) → ask user to confirm it fails → commit
- [ ] Write minimal implementation (**green**) → ask user to confirm it passes → commit
- [ ] Refactor: suggest improvements, ask user for more → apply → ask user to confirm still green → commit
