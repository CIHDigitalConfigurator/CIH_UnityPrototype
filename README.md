# CIH_UnityPrototype
CIH Unity based prototype of an example of a configurator

Currently using version 2020.1.15f1

### PRIMARY FUNCTIONALITY AND INTERFACE ROADMAP

1. Basic interface updates
   - Add cancel button to input field (cancel creating room)
   - Add warnings for height and aspect ratios
   - Add adjacencies check
   - Allow for creating non-compliant rooms
   - Add reverse functionality - delete room (reinstating tiles)
2. Serialise room output
3. Add floor by floor functionality (subtask list to be expanded)
   - Floor by floor display
   - Copy arrangement between the floors
   - Floor UI
4. Assemble a warning log that you can pull up via the UI
5. Carry out UI design exercise with the graphics team
6. Introduce split screen and associated UI controls
7. Interface updates part II
   - Group selection
   - Alter how the name is showing up
   - Add block to buttons during pause and debug
8. Add functionality associated to external voids/ facade sculpting



### EXTENDED DEVELOPMENT PLAN

1. Apply the above functionality
2. Add reference to framework verification process (through API calls)
3. Speckle and Sharepoint - full integration
4. Automated layout proposal from GH -> potentially rebuild in Unity
5. Additions to Unity configurator allowing for circulation connectivity escape route modelling (phase II)



### REFERENCE IMPLEMENTATION - OBJECTS AND THEIR PARAMETERS

#### Object parameters

1. 'Tile'/ 'voxal' IN (in the reference implementation passed from GH to Unity)
   - centre point (Vector3)
   - width (float)
   - depth (float)
   - rotation (Vector3 - Euler)
   - level (not obligatory - subject to centre point analysis if not given) - (float)
   - ID (string/ ID)
2. 'Room type' IN:
   - name (string)
   - min area (float)
   - "banned adjacencies" (List<string>)
   - min height (float)
   - max aspect ratio (float)
   - PHASE II: circulation (bool) 
   - PHASE II: occupancy density (float)
   - PHASE II: other inputs (doors/ openings definitions etc) (...)
   - ID (string/ ID)
3. 'Room' OUT:
   - name (string)
   - type (enum)
   - vertices (List<Vector3>)
   - edges - internal/ external (List<(int,int)>)
   - height (float)
   - level (float)
   - adjacencies (List<string/IDs>)
   - warning log (List<string> or dict)
   - PHASE II: outputs (...)

#### Proposed 'room type' list:

- office
- classroom
- gym
- assembly hall
- prep kitchen
- canteen
- plant room
- common room
- external void
- corridor
- staircase
- lifts
- core
