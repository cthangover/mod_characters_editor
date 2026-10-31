# Characters Editor — sửa nhân vật của mod bằng biểu mẫu

Characters Editor sửa các nhân vật của một mod khi game đang chạy: tên bằng mọi ngôn ngữ của mod, chân dung, sprite đi bộ cho bản đồ RPG, các con số chiến đấu, tỉ lệ chiêu mộ và hành động chiến đấu. **Lưu** ghi các tệp của mod và game dùng thay đổi ngay.

Nó mở từ hộp công cụ (chiếc búa trên thanh công cụ) dưới tên **Nhân vật**.

## Một chỉnh sửa và kết quả trong game

Trong RPG Plus, anh hùng của ngôi làng là nhân vật `rpg_hero` của mod `rpg_plus_game`. Chọn mod đó ở trên, chọn **Anh hùng**, gõ một tên tiếng Anh mới rồi nhấn **Lưu** (Ctrl+S):

![Cửa sổ Nhân vật trên rpg_plus_game: danh sách Aldric · rpg_hero, Slime · rpg_slime, Sói · rpg_wolf; biểu mẫu của rpg_hero với khóa tên rpg_demo/character/hero, văn bản (en) Aldric và sáu ngôn ngữ còn lại giữ nguyên, Chân dung characters/village/hero cùng hình của nó, Charset (RPG) trống, Cấp độ 1, Máu 24, Tấn công 6, Phòng thủ 0, Điểm hành động 1, Kinh nghiệm 0, Tỉ lệ chiêu mộ 0. Dòng trạng thái: "Đã lưu: 0 tệp, 1 văn bản trong mod rpg_plus_game; game đã đọc lại dữ liệu".](assets/editor-saved.jpg)

Chỉ dòng tiếng Anh trong `locale/en.properties` của mod thay đổi. Ngay sau đó, thành viên tổ đội trên bảng nhân vật:

![Bảng hành động nhân vật: thành viên tổ đội giờ tên là Aldric (rpg_hero, HP 42/42, AP 1, ATK 12, DEF 4) với Đánh cơ bản, Phòng thủ và Cú đánh mạnh.](assets/panel-after.jpg)

Bảng hiện HP 42 và ATK 12 trong khi tệp ghi 24 và 6: trong RPG Plus, lớp và chỉ số của anh hùng (Stats Engine với Classic RPG Rules) tính các con số chiến đấu và ghi đè lên giá trị của tệp. Trong game không có chỉ số thì dùng đúng các con số trong tệp.

## Biểu mẫu

- **Id** — id của nhân vật; kịch bản, tổ đội, trận đấu và bản đồ tìm nó qua id này.
- **Tên** — khóa dịch và một dòng cho mỗi ngôn ngữ của mod.
- **Chân dung** — hình trên thẻ và bảng; **…** mở lưới để chọn.
- **Charset (RPG)** — sprite đi bộ trên bản đồ RPG (3 khung × 4 hướng).
- **Cấp độ**, **Máu**, **Tấn công**, **Phòng thủ**, **Điểm hành động**, **Kinh nghiệm** — các con số chiến đấu của nhân vật.
- **Tỉ lệ chiêu mộ** — từ 0 đến 100: khả năng thu nhận nhân vật sau khi đánh bại nó (xem Recruiting).
- **Hành vi** và **Hành động** — nhân vật đánh thế nào và có những hành động chiến đấu nào.
- **Trường của mod khác** — các trường mà mod khác mô tả cho nhân vật trong lược đồ của họ (ví dụ lớp, chỉ số và đặc tính của Stats Engine) hiện trong mục riêng.
- **Các trường khác** — phần còn lại của JSON nhân vật, sửa dưới dạng JSON và giữ nguyên như cũ.

Thanh công cụ: **Mới** (Ctrl+N), **Sao chép** (Ctrl+D), **Xóa**, **Hoàn tác**/**Làm lại** (Ctrl+Z / Ctrl+Y), **Lưu** — chỉ những tệp thay đổi được ghi; văn bản vào các tệp bản địa hóa của mod.

## Nhân vật của mod khác

Danh sách hiển thị nhân vật của mod đã chọn. Đánh dấu **Entries of other mods** để thêm nhân vật của các mod phụ thuộc, có ghi `↗ chủ sở hữu`. Sửa một nhân vật như vậy sẽ ghi một bản vá vào mod của bạn (`patches/character/<mod của bạn>.json`) chỉ với các giá trị đã đổi; tệp của chủ sở hữu giữ nguyên từng byte. **Reset to inherited** xóa bản vá, còn **↺** cạnh một trường xóa giá trị của bạn ở trường đó. Chỉ ghi được vào các mod được cài đặt dưới dạng thư mục.

## Khối đội hình

Bên dưới các trường, khối đội hình hiển thị hai điều:

- **Đội trưởng** — nhân vật mà người chơi dùng hồ sơ khi đi trên bản đồ RPG. Đội trưởng là nhân vật mà `stats.player` của trò chơi chỉ định; nếu không có, là nhân vật đầu tiên của đội hình chiến đấu. Khối hiển thị *Party leader* hoặc *Not the party leader* cho nhân vật đang mở. Hiện chưa có nút đặt đội trưởng khác: đó là thiết lập của bộ sưu tập, đổi qua trình quản lý mod.
- **Các chỉ số chiến đấu đã tính** — nếu nhân vật có `Stats`, hiển thị từng chỉ số kèm giá trị và số chiến đấu. Gợi ý nói rằng máu, công và thủ lấy từ chỉ số, nên hãy sửa chỉ số chứ không sửa các trường này. Khi có `Stats`, **Health**, **Attack** và **Defence** chỉ đọc. Không có `Stats` thì khối nói rõ điều đó và chỉ số chiến đấu là các trường phía trên.
- **Gia nhập đội khi nói chuyện** — đánh dấu để ghi kịch bản gia nhập `scenarios/<mod của bạn>/party/<id>.scenario`: nó đặt cờ `<id>_joined` thành 1 và thêm nhân vật vào đội (`action party.add`). Bỏ đánh dấu sẽ xóa tệp ngay. Đội trưởng không bao giờ gia nhập (ô này bị khóa). Để nhân vật thành đồng đội trên bản đồ, hãy đặt nó từ tab **Nhân vật trò chơi** của Trình chỉnh sửa bản đồ RPG: nói chuyện với actor đã đặt sẽ khiến nó gia nhập, actor rời ô của nó, và đồng đội đi theo sau nhân vật chính.

## Khi có trục trặc

1. **Đổi máu hay tấn công không có tác dụng.** Game tính các con số từ chỉ số, như RPG Plus làm với tổ đội của nó.
2. **Nhân vật không có trong bảng đối tượng của Trình chỉnh sửa bản đồ RPG.** Tab **Nhân vật trò chơi** chỉ liệt kê nhân vật có bộ hình đi (**Charset**); không có thì không đặt được. Tab **Nhân vật** (actor) liệt kê các đối tượng actor của `rpg_object`, không phải nhân vật của trò chơi.
3. **Danh sách mod trống.** Mod đóng gói không sửa được.
