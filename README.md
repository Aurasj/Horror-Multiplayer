# Horror‑Multiplayer

A prototype multiplayer shooter game using Steam P2P communication in Unity.

---

## 📝 Description

This project is a multiplayer shooter prototype built with Unity that uses Steam P2P for networking.  
It shows how to use Steam’s peer‑to‑peer API for players to connect and play together in a basic environment.

---

## 📝 How P2P Works

This game uses **peer-to-peer (P2P) networking**, which means that players connect **directly to each other** through Steam, without needing a central server.  

### How it works:
- Each player runs a client that can send and receive game data (like positions, shots, and actions) directly to/from other players.  
- Steam handles NAT punch-through and matchmaking, so players behind routers or firewalls can still connect.  
- Because it's P2P, there’s **no server to host**, which saves money and setup time.

### What you need for a smooth experience:
- **Stable internet connection**: Since all data goes directly between players, high latency or packet loss can cause lag.  
- **Reasonable bandwidth**: Each client sends updates to other players, so very slow connections may affect gameplay.  
- **Steam running**: Steam must be logged in for P2P to work properly.

**Note:** P2P is great for small games and prototypes, but for large-scale games, a dedicated server can improve stability and cheat prevention.

---

## 🔧 Features

- Multiplayer support via Steam P2P transport  
- No server required — purely peer-to-peer  
- Unity C# scripts for player movement, shooting & networking  
- Basic scene and setup to test player vs player interaction  
- **Settings Menu**:  
  - Resolution selection  
  - Graphics quality settings  
  - Fullscreen toggle  
  - VSync toggle  
  - Shadows toggle  
  - FPS display toggle  
- **Rebinding controls** using Unity Input System  
- **Inventory system** for in-game items (if implemented)  


---

## 🚀 Getting Started

1. Clone the repository:  
```bash
git clone https://github.com/Aurasj/Horror‑Multiplayer
```
2. Open the project in Unity (compatible version as per the project settings).
3. Ensure your Steamworks SDK and P2P transport are configured properly.
4. Open the sample scene and run the game — two players should be able to connect via Steam.

---

⚠️ Important Notes

This is a prototype and may not be fully optimized or feature‑complete.

The transport layer (Steam P2P) is tied to Unity and Steamworks — may require updates for newer Unity/Steam versions.

Use it as a learning tool or base for your own multiplayer game.

---

👤 Author

Aurasj

---

## 🔓 Usage

Feel free to use, modify, and extend this project for **learning and personal use**.  

If you use this project and manage to **go beyond the prototype** or create something cool from it, I’d love to hear about it — it would make me really happy! (:

