# Honours Project

NPCs randomly spawn in one of the spawn points.
Criminal follows set path and triggers NPC reaction. 

NPCs have one of three reactions:
- Fight,
- Cower,
- Flee.

Fuzzy logic has arrogance implemented, with a range of 0-10, with the following reactions: 
- 8-10: Fight
- 0-7: Flees (this is prototype, no cower implemented for now). 

NPC_BinaryBehaviour has a Boolean (is_fuzzy) to control whether binary or fuzzy simulation. 