# AN END is a new Beginning

[Brackeys Game Jam 2023.1](https://itch.io/jam/brackeys-9)

Unity `2021.3.18f1` (LTS)  
2D URP



## Idea / Mechanics

横板弹幕射击，没有卷轴，地图分成一些独立的房间（每个大关中彼此相连）。

- 每个大关彼此独立（？），拥有各自的关底boss；教学关会短一些。

玩家的操作（移动、攻击）会被记录下来。击破关底boss后，玩家将变身，获得boss的能力并从终点返回起点。

- 每一个玩家经过的房间中都会产生玩家的幻影，重放玩家的操作。

  - 重放完毕后，玩家幻影返回起点，进入break状态。损失一定HP / 经过一定时间后replay。

- 玩家需要在正向流程中限制自己的火力，给返回的过程降低难度。

  > - 如果之前的自己太强了打不过怎么办x
  > - 该怎么避免玩家摆烂x
  > - 玩家有无体术判定（好鬼畜哦



## Instructions

#### Tilemap

- 编辑：打开`Window > 2D > Tile Palette`

#### 物理

- 所有需要与角色交互的实心区域`Layer`设置为`3: Obstacles`



## Credits

### Libraries

[Ultimate 2D Controller](https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller) by Matthew-J-Spencer

### Sprites / Tilemaps

[8x8 1bit Dungeon Tilemap](https://pixelhole.itch.io/8x8dungeontilemap)



### Audio

### Misc.
