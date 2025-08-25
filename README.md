GUMSHOE – Multiplayer Social Deduction Game

📖 Overview

GUMSHOE is a multiplayer social-deduction game inspired by Secret Hitler, Werewolf, and Cluedo.
It reimagines traditional turn-based deduction into a dynamic first-person exploration game with online multiplayer support.

Players gather clues, collaborate, and use unique abilities to uncover the mystery. At the same time, hidden saboteurs can disrupt investigations through sabotage events, creating tension and strategy.

🎮 Core Features

Multiplayer Networking: Built with Photon Unity Networking (PUN2) for room-based matchmaking, synchronization, and events.

Cross-Platform Voice Chat: Real-time communication powered by Photon Voice.

User Authentication & Data Persistence: Integrated with PlayFab for login/registration, character selection, and secure backend storage.

Invite Codes & Lobbies: Custom room codes for private games; public matchmaking supported.

Lag Compensation: Network interpolation and prediction to ensure smooth gameplay.

Special Abilities: Includes disruptive mechanics such as Lights-Out events, forcing players to adapt strategies under blackout conditions.

Persistent Player Data: Character choices and preferences saved across sessions.

🛠️ Tech Stack

Engine: Unity3D (HDRP pipeline for detailed lighting and atmosphere)

Networking: Photon Unity Networking (PUN2) + Photon Voice

Backend Services: PlayFab (authentication, user accounts, persistent player data)

Programming Language: C#

Version Control: Git / GitHub

🔑 Gameplay Loop

Investigation Phase:

Players explore the environment, gather clues, and solve minigames (e.g., water jug puzzle).

Tools like UV light reveal hidden evidence.

Discussion Phase:

Players share findings and collaborate to piece together the mystery.

Deduction and strategy are key.

Sabotage & Abilities:

Special powers such as Lights-Out create disruption and tension.

Saboteurs earn points for successfully halting investigations or causing minigame failures.

Resolution:

Players attempt to solve the case collectively.

Replayability supported via randomized case generation (planned future expansion).

🌐 Networking Implementation

Lobby System: Players join/create rooms via Photon; late joiners handled with custom properties.

Invite Codes: Custom alphanumeric codes for quick private matches.

Synchronization: Player avatars, transforms, and animations synced across all clients.

Lag Compensation: Network time offsets, interpolation, and Rigidbody/Transform sync.

Event System: Centralized RaiseEvent manager to ensure safe, consistent handling of networked events.

📂 Data Design

Case System: ScriptableObject-based CaseData models (Motive, Weapon, Suspect).

Designed for periodic live-ops updates and AI-assisted narrative generation.

Custom Unity Editor tooling for designers to create coherent cases quickly.

🧪 Results

Stable gameplay at 60fps / 1080p, supporting 4–8 concurrent players.

Networking robust under separate networks with no major issues.

User Feedback:

Positive reception for intuitive objectives and clue-gathering.

Highlighted need for more incentives to use sabotage/abilities.

Minigame timer mechanics flagged for refinement.

🚀 Future Work

Adaptation for PlayStation 5 dev kits with controller and console networking tests.

Completion of the random case generator for infinite replayability.

Expanded toolset and abilities to enrich sabotage/strategy dynamics.

Improved level design to create natural opportunities for abilities.

📜 References

Photon Unity Networking (PUN2): photonengine.com/pun

It Takes Two (Hazelight Studios, 2021) – cooperative design reference

Among Us (InnerSloth, 2018) – social deduction and lobby code inspiration

👥 Authors

Damian Boguslawski – K2369482

Darshan Radhakrishnan – K2288661
