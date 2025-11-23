# Honours Project

NPCs randomly spawn in one of the spawn points.
Criminal follows set path and triggers NPC reaction. 

NPCs have one of three reactions:
- Fight,
- Cower,
- Flee.

Fuzzy logic has arrogance implemented, with a range of 0-10, with the following reactions: 
- 8-10: Fight
- 3-7: Flees 
- 0-2: Cowers

NPC_BinaryBehaviour has a Boolean (is_fuzzy) to control whether binary or fuzzy simulation. 