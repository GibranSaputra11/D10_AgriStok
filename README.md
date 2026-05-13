# Screenshot Hasil Menjalankan Sistem

## 1. Form Koneksi
<img width="915" height="543" alt="image" src="https://github.com/user-attachments/assets/8260034c-ead1-4528-a8ec-4edbf93e1fcd" />

## 2. Form Input Data
<img width="621" height="527" alt="image" src="https://github.com/user-attachments/assets/9ff0f800-631c-4734-97f4-e0d63922ae30" />



## 3. Form Tampilan Data
<img width="511" height="203" alt="image" src="https://github.com/user-attachments/assets/1a4451ce-f2b0-4f45-b0cc-e10a274d8fb8" />


## 4. Bukti Fungsionalitas (Insert, Update, Delete)
<img width="623" height="532" alt="image" src="https://github.com/user-attachments/assets/b573bd13-a8f7-4b81-950a-dbc5457b7c00" />

### Update
<img width="630" height="538" alt="image" src="https://github.com/user-attachments/assets/5a9368c8-3baf-4b96-b052-42c0a33831b0" />
<img width="621" height="528" alt="image" src="https://github.com/user-attachments/assets/cc79f409-4f26-4269-b23f-2f53a3039d6e" />

### Delete
<img width="684" height="536" alt="image" src="https://github.com/user-attachments/assets/5215f5d9-de55-4958-8abd-b8098c893e40" />
<img width="623" height="528" alt="image" src="https://github.com/user-attachments/assets/cf3ee4fc-923d-40f2-953b-cc17d6923d0e" />
<img width="629" height="534" alt="image" src="https://github.com/user-attachments/assets/a978dd05-b6f0-4f46-a768-54f4feb423f9" />

## 5. Sekenario SQL Injection
Pada kondisi normal, aplikasi C# bakal ngubah nama supplier sesuai nama yang diinput user. Misalnya user ngetik “Suka Maju”, nanti sistem bikin query buat nyari supplier itu lalu ngubah namanya jadi “HACKED_BY_GIBRAN”, jadi yang berubah cuma satu data aja. Tapi karena query dibuat dengan cara gabung teks langsung tanpa parameter pengaman, seperti "... WHERE Nama_Supplier = '" + txtNamaSupplier.Text + "'", apa pun yang diketik user bakal dianggap sebagai perintah SQL oleh Microsoft SQL Server. Padahal cara yang aman seharusnya menggunakan parameter seperti WHERE Nama_Supplier = @Nama. Nah di sini kami coba masukin input ' OR 1=1 -- ke TextBox. Akibatnya query yang tadinya cuma buat satu supplier berubah jadi perintah yang nge-update seluruh isi tabel Supplier. Bagian OR 1=1 bikin kondisi pencarian selalu benar, jadi semua data supplier ikut keubah jadi “HACKED_BY_GIBRAN”, sedangkan -- dipakai buat ngabaikan sisa query supaya tidak error. Karena ini cuma simulasi buat demo ke dosen dan bukan buat ngerusak aplikasi AgriStok permanen, sebelum percobaan dilakukan semua data supplier dibackup dulu ke tabel Supplier_Backup. Setelah simulasi selesai, data supplier yang berubah tadi dikembalikan lagi ke kondisi semula menggunakan UPDATE JOIN tanpa menghapus data asli karena tabel Supplier masih punya relasi dengan tabel Transaksi_In.

<img width="660" height="490" alt="image" src="https://github.com/user-attachments/assets/3580b45d-0f9b-4f26-a209-b355b74b6778" />
<img width="672" height="488" alt="image" src="https://github.com/user-attachments/assets/7e05cf2a-ae8c-4579-9865-0abe078f0c0a" />
<img width="606" height="486" alt="image" src="https://github.com/user-attachments/assets/135be60a-8758-40a3-b408-f48595d9158c" />
<img width="722" height="489" alt="image" src="https://github.com/user-attachments/assets/a3d655a2-9bc8-4948-9374-e0241f7c87cc" />
<img width="604" height="486" alt="image" src="https://github.com/user-attachments/assets/a48c1878-5488-4471-8f71-9652cc14e8c0" />









