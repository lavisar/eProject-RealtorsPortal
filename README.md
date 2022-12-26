# eProject-RealtorsPortal 
## Ghi chú github

***push 1 bản lỗi lên github và cần revert lại bản commit trước đó?
1. git log --oneline   -> để lấy ID của lần commit cần revert
2. git reset (ID đó) --hard

***Lỗi: "Pull is not possible because you have unmerged files"
1. commit the changes using: git add . && git commit -m "removed merge conflicts"
2. git pull

***Lỗi: Files have invalid value in path "<<<<<<< HEAD"
solve: https://social.msdn.microsoft.com/Forums/en-US/354f9b7a-1adb-410d-81ec-ae51fcc2e3dc/files-have-invalid-value-in-path-quotltltltltltltlt-headquot?forum=aspvisualstudio
1. Mở folder exploer > vào thư mục dự án và xóa thư mục obj đi
2. Mở VS và rebuid
