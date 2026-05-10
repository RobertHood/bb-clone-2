# 🟦 BB Clone 2 — Block Blast Clone (Unity)

> Một game xếp khối (block puzzle) lấy cảm hứng từ **Block Blast**, được xây dựng bằng Unity 6.

---

## 📋 Mục lục

- [Giới thiệu](#giới-thiệu)
- [Tính năng](#tính-năng)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt & Mở project](#cài-đặt--mở-project)
- [Cách chơi](#cách-chơi)
- [Cấu trúc project](#cấu-trúc-project)
- [Kiến trúc code](#kiến-trúc-code)
- [Hệ thống Augment](#hệ-thống-augment)
- [Assets & Tài nguyên](#assets--tài-nguyên)

---

## 🎮 Giới thiệu

**BB Clone 2** là một game puzzle xếp khối 2D trên lưới **10×10**. Người chơi kéo và thả các khối hình dạng khác nhau vào lưới. Khi một hàng hoặc cột được lấp đầy, các ô đó sẽ bị xóa và người chơi nhận điểm. Game kết thúc khi không còn khối nào có thể đặt được lên lưới.

---

## ✨ Tính năng

| Tính năng | Mô tả |
|-----------|-------|
| 🧩 **Kéo thả block** | Kéo block từ vùng lựa chọn và thả vào lưới |
| 🔍 **Preview vị trí** | Highlight ô hợp lệ khi đang kéo block |
| 📊 **Hệ thống tính điểm** | Cộng điểm theo số ô đặt được và số hàng/cột xóa |
| 🎲 **Spawn thông minh** | Spawn 3 block khác nhau mỗi lượt, ưu tiên block nhỏ hơn (weighted random) |
| ⚡ **Hệ thống Augment** | Chọn 1 trong 3 hiệu ứng đặc biệt khi đạt điều kiện |
| 🔊 **Âm thanh** | Hiệu ứng âm thanh khi đặt block và khi game over |
| 💀 **Game Over** | Phát hiện tự động khi không còn vị trí hợp lệ |
| 🔄 **Restart** | Khởi động lại game từ màn hình game over |

---

## 🛠️ Yêu cầu hệ thống

| Yêu cầu | Phiên bản |
|---------|-----------|
| **Unity Editor** | `6000.1.6f1` (Unity 6) |
| **Render Pipeline** | Universal Render Pipeline (URP) `17.2.0` |
| **Input System** | New Input System `1.14.2` |
| **TextMesh Pro** | Tích hợp sẵn (UGUi `2.0.0`) |
| **2D Packages** | Animation, Tilemap, SpriteShape, Aseprite... |

---

## 🚀 Cài đặt & Mở project

1. **Clone repository:**
   ```bash
   git clone https://github.com/RobertHood/bb-clone-2.git
   ```

2. **Mở bằng Unity Hub:**
   - Mở **Unity Hub** → chọn **Open** → trỏ đến thư mục `bb-clone-2`
   - Đảm bảo đã cài **Unity 6 (6000.1.6f1)**

3. **Mở Scene game:**
   - Trong Project window, điều hướng đến `Assets/Scenes/`
   - Mở file `Game.unity`

4. **Chạy game:**
   - Nhấn nút **Play** ▶️ trong Unity Editor

> ⚠️ **Lưu ý:** Đảm bảo Unity tự động import tất cả packages từ `Packages/manifest.json` trước khi chạy.

---

## 🕹️ Cách chơi

1. **Mỗi lượt**, 3 block khác nhau sẽ xuất hiện ở vùng lựa chọn phía dưới.
2. **Kéo thả** một block vào lưới 10×10. Ô hợp lệ sẽ được highlight khi đang kéo.
3. Nếu block **không thể đặt** được vào vị trí đang kéo, nó sẽ tự trở về vị trí cũ.
4. Khi **một hàng hoặc cột đầy**, tất cả ô trong hàng/cột đó sẽ bị xóa và bạn nhận điểm.
5. Sau khi cả 3 block đã được đặt, **3 block mới** sẽ được spawn.
6. **Game Over** xảy ra khi không còn block nào trong lượt hiện tại có thể đặt vào lưới.

---

## 📁 Cấu trúc project

```
bb-clone-2/
├── Assets/
│   ├── Audio/                          # Âm thanh game (MP3)
│   │   ├── *_Game_Over_05.mp3          # Nhạc game over
│   │   └── *_Pop_02.mp3                # Âm thanh đặt block
│   ├── Block/                          # Prefabs của các loại block
│   │   ├── I_Block.prefab              # Khối I thẳng đứng
│   │   ├── O_Block.prefab              # Khối vuông 2x2
│   │   ├── 3x3O_Block.prefab           # Khối vuông 3x3
│   │   ├── T_Block.prefab              # Khối chữ T
│   │   ├── L_Block.prefab              # Khối chữ L và các biến thể
│   │   ├── Z_Block.prefab              # Khối chữ Z và các biến thể
│   │   └── ...                         # ~24 loại block khác nhau
│   ├── Scripts/
│   │   ├── GridManager.cs              # Quản lý lưới chính
│   │   ├── BlockSpawner.cs             # Sinh block cho mỗi lượt
│   │   ├── BlockData.cs                # Dữ liệu & kéo-thả block
│   │   ├── AudioManager.cs             # Singleton quản lý âm thanh
│   │   ├── AugmentSpawner.cs           # Spawn các lựa chọn Augment
│   │   └── Augments/
│   │       ├── Augment.cs              # UI & logic Augment
│   │       ├── AugmentEffect.cs        # Base class (ScriptableObject)
│   │       ├── HigherScore.cs          # Augment: tăng hệ số điểm
│   │       ├── LuckyClear.cs           # Augment: tự xóa ngẫu nhiên
│   │       ├── NothingHappens.cs       # Augment: không có hiệu ứng
│   │       └── LuckyClearController.cs # Runtime controller cho LuckyClear
│   ├── Scenes/
│   │   ├── Game.unity                  # Scene game chính ✅
│   │   └── SampleScene.unity           # Scene mẫu
│   ├── Sprites/                        # Hình ảnh tile (PNG + Tile assets)
│   ├── Resources/                      # Tài nguyên runtime
│   ├── Settings/                       # Cấu hình URP
│   └── Background.png                  # Ảnh nền game
├── Packages/
│   └── manifest.json                   # Danh sách Unity packages
├── ProjectSettings/                    # Cài đặt project Unity
└── README.md
```

---

## 🏗️ Kiến trúc code

### Sơ đồ tương tác các hệ thống chính

```
BlockData (Drag & Drop)
    │── OnBeginDrag ──► GridManager.StartDrag()
    │── OnDrag       ─► GridManager.UpdateDragPosition()
    └── OnEndDrag   ──► GridManager.EndDrag()
                              │
                         [Validate & Snap]
                              │
                         GridManager.CheckAndClear()
                              │
                    ┌─────────┴──────────┐
               CollectFullRows()    CollectFullCols()
                    └─────────┬──────────┘
                         ClearCells()
                              │
                    CheckAllBlockPlaceable()
                              │
                         [GameOver?]

BlockSpawner ──► GenerateSmartBlock() (weighted random)
             └── SpawnBlock() → 3 block khác nhau mỗi lượt
```

### Các class chính

| Class | Vai trò |
|-------|---------|
| `GridManager` | Quản lý toàn bộ lưới 10×10, drag-drop, tính điểm, phát hiện game over |
| `BlockSpawner` | Spawn 3 block ngẫu nhiên có trọng số; đảm bảo tính đa dạng và tính đặt được |
| `BlockData` | Xử lý sự kiện con trỏ (IPointerDown, IBeginDrag, IDrag, IEndDrag) |
| `AudioManager` | Singleton quản lý âm thanh (đặt block, game over) |
| `AugmentSpawner` | Hiển thị 3 lựa chọn Augment ngẫu nhiên |
| `Augment` | UI Augment: hiển thị tên, mô tả, hệ số điểm và xử lý click |
| `AugmentEffect` | ScriptableObject base class cho các hiệu ứng Augment |

---

## ⚡ Hệ thống Augment

Augment là các hiệu ứng đặc biệt có thể được kích hoạt trong game. Mỗi Augment là một **ScriptableObject** kế thừa từ `AugmentEffect`.

| Augment | Hiệu ứng |
|---------|----------|
| **HigherScore** | Tăng hệ số nhân điểm (score multiplier) |
| **LuckyClear** | Có xác suất (~5%) tự xóa toàn bộ bảng sau mỗi lần clear hàng/cột |
| **NothingHappens** | Không có hiệu ứng (placeholder/troll) |

Để thêm Augment mới:
1. Tạo class kế thừa `AugmentEffect` với `[CreateAssetMenu]`
2. Override phương thức `Apply(GameObject target)`
3. Tạo asset từ menu **Assets → Create → Augments → ...**
4. Thêm asset vào danh sách `augmentEffects` của `AugmentSpawner`

---

## 🎨 Assets & Tài nguyên

| Loại | Mô tả |
|------|-------|
| **Tile Sprites** | `highlighted_tile.png`, `tile_background60x60.png`, `red_tile.png` |
| **Block Prefabs** | ~24 hình dạng block khác nhau (I, O, T, L, Z và các biến thể) |
| **Âm thanh** | Hiệu ứng đặt block (Pop) & Game Over (Casino Electronic) |
| **Render Pipeline** | Universal Render Pipeline (URP) với Post-processing |
| **UI** | TextMesh Pro cho hiển thị điểm và tên Augment |

---

## 📝 Ghi chú phát triển

- Lưới game sử dụng Unity **Tilemap** với tọa độ `(0,0)` đến `(9,9)`.
- Block được spawn với **weighted random**: block càng nhiều ô → xác suất spawn càng thấp, đảm bảo trải nghiệm cân bằng.
- `AudioManager` sử dụng pattern **Singleton + DontDestroyOnLoad** để tồn tại xuyên scene.
- Hệ thống **game over** kiểm tra tất cả block hiện tại xem có thể đặt vào bất kỳ vị trí nào còn trống không.

---

*Made with ❤️ using Unity 6 — URP 2D*
