# 🚀 The Last Dreadnought 

**The Last Dreadnought** is a fast-paced 3D action game developed with **Unity 6**, inspired by fast FPS mechanics and arcade tank combat.  
You play as the last surviving dreadnought, fighting endless waves of enemies while upgrading your vehicle through a perk system.

---

## 🎮 Gameplay

- Fast-paced movement and shooting
- Tank-based combat
- Critical hits, armor & penetration system
- Perk-based progression (roguelike style)
- Enemy waves with increasing difficulty

---

## 🧠 Core Systems

### Combat System
- Shell-based projectile system
- Armor vs penetration logic
- Damage based on caliber
- Critical hits with pity system
- Life steal and Bloodbath perks

### Perk System
- ScriptableObject-based perks
- Random perk selection between levels
- Permanent and unique perks
- Stat modifications:
  - Damage
  - Crit chance / damage
  - Armor
  - Max health
  - Reload time
  - Movement speed
  - Caliber

### Player Systems
- PlayerHealthManager (IDamageable)
- GunManager (weapon stats, crit, pity)
- Movement system
- XP and level system
- UpgradeManager

---

## 🛠 Tech Stack

- **Engine:** Unity 6
- **Language:** C#
- **Architecture:**
  - ScriptableObjects
  - Managers (GameManager, UpgradeManager, etc.)
  - Interfaces (IDamageable)
  - Event-driven game states

---

## 📂 Project Structure

---

## 🏗 Architecture Highlights 

- Damage system fully decoupled using `IDamageable`
- Centralized GameState machine
- Data-driven perks via ScriptableObjects
- No hard references between combat systems
- Extensible perk effect system

---

## 🧪 Features Implemented

- [x] Damage & armor system  
- [x] Critical hits + pity system  
- [x] Perk selection UI  
- [x] Life steal & Bloodbath  
- [x] Enemy drops  
- [x] Health bar with chip effect  
- [x] Wave progression  

---

## 🧩 In Progress / Planned

- Boss fights
- More enemy types
- Procedural levels
- Sound design polish
- Visual feedback (hit markers, screen shake)
- More weapons

---

## 📸 Screenshots



---

## 🧑‍💻 Author

Developed by **Clément Faivre**  
Student / Indie Game Developer

- Unity
- Gameplay Programming
- Systems Design

---

## 📜 License

This project is for learning and portfolio purposes.  
Feel free to fork or study the code.
