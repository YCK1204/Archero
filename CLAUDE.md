# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Archero Unity Game** - A 2D action game built with Unity 2022.3.17f1 using Universal Render Pipeline. This is a team development project with individual developer workspaces and a feature-based git workflow.

## Common Development Commands

### Unity Development
- **Open Project**: Open `Archero/` folder in Unity Hub
- **Build**: Use Unity Editor's Build Settings (File → Build Settings)
- **Test Scenes**: MainScene (integrated), HSscene, JMscene (individual testing)

### Git Workflow
- **Main Branch**: `main` (production)
- **Development Branch**: `develop` (integration) 
- **Feature Branches**: Named after developers (Yechan, Hyeonsu, Jeongmin, Yoon, Youngshin)
- **Current Active**: `Yechan` branch

## Architecture Overview

### Core Manager System (Singleton Pattern)
**GameManager** acts as the central hub coordinating all other managers:
- **UIManager**: UI system with base classes for PopUp/Window UIs
- **ResourceManager**: Async resource loading via Addressables
- **PoolManager**: Generic object pooling for performance
- **RewardManager**: Level progression and reward system

**BattleManager** handles combat registration:
- Maintains `Dictionary<Collider2D, Action<int, Vector3>>` for hit registration
- Use `RegistHitInfo()` to register combat units
- Use `Attack()` for damage dealing

### Key Systems Structure

#### Player System (`Assets/Scripts/`)
- **PlayerController**: Main orchestrator
- **PlayerInputHandler**: Unity Input System integration
- **PlayerMovingHandler**: Movement + dash mechanics
- **CharacterStats**: Level-based stat progression
- **PlayerCollision**: Environment interaction

#### Weapon System (`Assets/Scripts/WeaponScripts/`)
- **WeaponBase**: Abstract base for all weapons
- **RangedWeapon/MeleeWeapon**: Specific implementations
- **WeaponHolder**: Weapon management
- **Projectile**: Projectile behavior with pooling

#### Item System (`Assets/Scripts/Item/`)
- **ItemData**: ScriptableObject-based item definitions
- **CollectableItem**: Collection mechanics with magnetism
- Types: ArmorItemData, EXPItemData, PotionItemData, WeaponItemData

#### Enemy System (`Assets/Yoon/`)
- **Monster**: Main enemy controller
- **MonsterStateMachine**: State pattern AI (Patrol, Trace, Attack, Damaged, Die)
- **MonsterStat**: Enemy statistics
- **MobProjectile**: Enemy projectile system

#### Map Generation (`Assets/Scripts/Map/`)
- **AbstractDungeonGenerator**: Base for procedural generation
- **SimpleRandomWalkDungeonGenerator**: Random walk algorithm
- **TilemapVisualizer**: Tilemap rendering

### Developer Workspaces
- **Lee** (`Assets/WorkSpase/Lee/`): UI systems, managers, reward mechanics
- **Yoon** (`Assets/Yoon/`): Enemy AI, state machines, combat handlers
- **Others**: Individual scenes for testing (HSscene, JMscene)

## Key Design Patterns

1. **Singleton**: All managers inherit from `SingleTon<T>`
2. **State Machine**: Enemy AI uses state pattern with `IState` interface
3. **Object Pooling**: Generic `Pool<T>` class for projectiles/effects
4. **Observer**: UI event handling and item collection
5. **Strategy**: Different attack handlers (MeleeHandle, RangeHandle)

## Unity Package Dependencies

Key packages that define the tech stack:
- **Addressables** (1.21.21): Asset management and async loading
- **Input System** (1.7.0): Modern input handling
- **AI Navigation** (1.1.7): NavMesh pathfinding
- **URP** (14.0.9): Universal Render Pipeline for 2D
- **NavMeshPlus**: Enhanced 2D navigation from GitHub
- **TextMeshPro** (3.0.6): Text rendering
- **Test Framework** (1.1.33): Unit testing support

## Critical Development Notes

### Combat System Integration
- **Always register combat units** with BattleManager using `RegistHitInfo()`
- **Hit registration pattern**: `unitDict.ContainsKey(target)` before attacking
- **Pooling required** for projectiles to maintain performance

### Resource Loading
- **Use Addressables** for all runtime asset loading
- **Async patterns** implemented in ResourceManager
- **Never direct Resources.Load()** - use manager system

### Input Handling
- **Unity Input System** is configured in `Assets/PlayerInput/`
- **PlayerInputControl.inputactions** defines input mappings
- **Handler pattern** separates input from movement logic

### Scene Management
- **MainScene**: Primary integrated scene
- **Individual scenes**: For isolated feature testing
- **Navigation**: NavMesh baked for each scene

## Data Flow Architecture

```
Input → PlayerInputHandler → PlayerController → Game Systems
GameManager → [UI/Resource/Pool/Reward]Manager → Specific Systems
BattleManager → Combat Registration → Damage Calculation → Stats Update
```

## Performance Considerations

- **Object Pooling**: Mandatory for projectiles, effects, and enemies
- **Addressable Loading**: Async to prevent frame drops  
- **State Machine**: Efficient AI processing with minimal Update() calls
- **Generic Pooling**: `Pool<T>` provides reusable optimization

## Current Development Status

Recent focus areas based on git history:
- Item magnetism and collection system
- Addressable asset conflict resolution
- Scene consolidation into MainScene
- Individual feature development in parallel branches

## Namespace Organization

- **Lee.Scripts**: UI and manager systems
- **Assets.Define**: Core singleton managers
- **No namespace**: Player, weapon, and item systems
- **Individual workspaces**: Developer-specific namespacing

## Language and Communication Conventions

- **Code Communication**: 
  - 나 한국인이니까 한국어로 답해