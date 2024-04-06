# Changelog

All notable changes to this package will be documented in this file.


## [Unreleased]
- Added new component `Helpers > ClampPosition`
- Added new component `Helpers > ClampScale`


## [0.4.0] - 2024-04-05
### Added / Updated / Fixed
- Actions - completely refactored logic to make instant actions executed in a single update or start event
- Actions - force execute ActionInnerUpdate() after ActionInnerStart() calls
- Improved performance for calculating data for points in a field
- `AddForce` (Rigidbody) - added new action to add force on `RigidBody`
- `AddTorque` (Rigidbody) - added new action to add torque on `RigidBody`
- `Destroyer` - review component structure, review custom Destroy.. methods
- `FindGameObjectAction` - added new action to find GameObject
- `FindGameObjectAction` - added `scope` to find object in `All Scene`, `Only in Parents`, `Only in Children`
- `Follow` & `LookAt` - set default value for `updateInEditor` field as false
- `Follow` - remove auto-set offset on init object to follow
- `LockTransform` - Ignore `lock scale` for `World` space
- `OnColliderEvent` - renamed + added Enabled-checkbox in UnityEditor to turn on/off event from editor
- `SpawnAction` & `Spawner` - added activateInstance property. Allow activate disabled object on spawn instances
- `SpawnAction` & `Spawner` - remove `resetPosition` field. Position will be auto reset to the zero+offset
- `SpawnAction` & `Spawner` added 2 new modes - spawn inside `SphereVolume` & `BoxVolume`
- `Tint` - make it works on URP & HDRP
- Bugfix annotation warning on hide gizmo icons

### Editor
- Actions - add special events on click on action icon: `Alt`+`Click` = unlink action; `Alt`+`Shift`+`Click` = unlink and delete action component
- `ActionsPopup` - added new group "GameObject" and move some components in it
- `Debugger` - review fields in editor
- `Spawner` & `Destroyer` - update hint messages in UnityEditor

### Others
- Big review all Demos
- Update project to use URP

### Experimental
- Added `Variables` (float)
- Added `Variables` > `NumericVariableAction` to change `NumbericVariables`
- Added `Variables` > `NumericToString` convertor



## [0.3.2] - 2024-01-05
- Fix: TransformRandomAction - correct use target object reference
- Fix: TransformUpdateAction - correct use target object reference



## [0.3.1] - 2024-01-04
- Inner code optimizations



## [0.3.0] - 2024-01-04
- Base version
