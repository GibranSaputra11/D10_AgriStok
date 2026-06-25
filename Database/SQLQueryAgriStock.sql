/* ============================================================================
   1. DATABASE
   ============================================================================ */
CREATE DATABASE GudangPertanianDB;
GO

USE GudangPertanianDB;
GO


/* ============================================================================
   2. TABEL
   ============================================================================ */

-- 2.1 Kategori
CREATE TABLE Kategori (
    Id_Kategori VARCHAR(10) PRIMARY KEY,
    Nama_Kategori VARCHAR(50) NOT NULL,
    CONSTRAINT UQ_Kategori UNIQUE (Nama_Kategori)
);
GO

-- 2.2 Satuan
CREATE TABLE Satuan (
    Id_Satuan VARCHAR(10) PRIMARY KEY,
    Nama_Satuan VARCHAR(50) NOT NULL,
    CONSTRAINT UQ_Satuan UNIQUE (Nama_Satuan)
);
GO

-- 2.3 Supplier
CREATE TABLE Supplier (
    Id_Supplier VARCHAR(10) PRIMARY KEY,
    Nama_Supplier VARCHAR(100) NOT NULL,
    Alamat_Supplier VARCHAR(255),
    NoTlp_Supplier VARCHAR(20),
    CONSTRAINT UQ_Supplier UNIQUE (Nama_Supplier),
    CONSTRAINT CHK_Supplier_Id CHECK (LEN(Id_Supplier) > 0)
);
GO

-- 2.4 KelompokTani
CREATE TABLE KelompokTani (
    Id_Kelompok VARCHAR(10) PRIMARY KEY,
    Nama_Kelompok VARCHAR(100) NOT NULL,
    Alamat_Kelompok VARCHAR(255),
    NoTlp_Kelompok VARCHAR(20),
    CONSTRAINT UQ_KelompokTani UNIQUE (Nama_Kelompok)
);
GO

-- 2.5 Barang (kolom Foto & UNIQUE Nama_Barang sudah digabung dari ALTER TABLE)
CREATE TABLE Barang (
    Id_Barang VARCHAR(10) PRIMARY KEY,
    Nama_Barang VARCHAR(100) NOT NULL,
    Id_Satuan VARCHAR(10),
    Id_Kategori VARCHAR(10),
    Stok_Barang INT DEFAULT 0,
    Foto VARBINARY(MAX) NULL,
    FOREIGN KEY (Id_Satuan) REFERENCES Satuan(Id_Satuan),
    FOREIGN KEY (Id_Kategori) REFERENCES Kategori(Id_Kategori),
    CONSTRAINT UQ_Barang UNIQUE (Nama_Barang)
);
GO

-- 2.6 Transaksi_In
CREATE TABLE Transaksi_In (
    Id_In VARCHAR(10) PRIMARY KEY,
    Id_Supplier VARCHAR(10),
    Tgl_In DATE,
    Total_Barang_In INT,
    FOREIGN KEY (Id_Supplier) REFERENCES Supplier(Id_Supplier)
);
GO

-- 2.7 Detail_In
CREATE TABLE Detail_In (
    Id_In VARCHAR(10),
    Id_Barang VARCHAR(10),
    Subtotal_In INT,
    PRIMARY KEY (Id_In, Id_Barang),
    FOREIGN KEY (Id_In) REFERENCES Transaksi_In(Id_In),
    FOREIGN KEY (Id_Barang) REFERENCES Barang(Id_Barang)
);
GO

-- 2.8 Transaksi_Out
CREATE TABLE Transaksi_Out (
    Id_Out VARCHAR(10) PRIMARY KEY,
    Id_Kelompok VARCHAR(10),
    Tgl_Out DATE,
    Total_Barang_Out INT,
    FOREIGN KEY (Id_Kelompok) REFERENCES KelompokTani(Id_Kelompok)
);
GO

-- 2.9 Detail_Out
CREATE TABLE Detail_Out (
    Id_Out VARCHAR(10),
    Id_Barang VARCHAR(10),
    Subtotal_Out INT,
    PRIMARY KEY (Id_Out, Id_Barang),
    FOREIGN KEY (Id_Out) REFERENCES Transaksi_Out(Id_Out),
    FOREIGN KEY (Id_Barang) REFERENCES Barang(Id_Barang)
);
GO

-- 2.10 Log_Error
CREATE TABLE Log_Error (
    Id_Error INT IDENTITY(1,1) PRIMARY KEY,
    Waktu_Error DATETIME DEFAULT GETDATE(),
    Pesan_Error VARCHAR(MAX)
);
GO

-- 2.11 Log_Aktivitas
CREATE TABLE Log_Aktivitas (
    Id_Log INT IDENTITY(1,1) PRIMARY KEY,
    Waktu_Log DATETIME DEFAULT GETDATE(),
    Aktivitas VARCHAR(255),
    Tabel_Terkait VARCHAR(50),
    Detail VARCHAR(MAX)
);
GO


/* ============================================================================
   3. VIEW
   ============================================================================ */

-- 3.1 Kelola Data (CRUD utama per modul)
CREATE VIEW vw_KelolaBarang AS
SELECT 
    b.Id_Barang, 
    b.Nama_Barang, 
    b.Id_Kategori, 
    k.Nama_Kategori, 
    b.Id_Satuan, 
    s.Nama_Satuan, 
    b.Stok_Barang,
    b.Foto
FROM Barang b
INNER JOIN Kategori k ON b.Id_Kategori = k.Id_Kategori
INNER JOIN Satuan s ON b.Id_Satuan = s.Id_Satuan;
GO

CREATE VIEW vw_KelolaSupplier AS
SELECT 
    Id_Supplier, 
    Nama_Supplier, 
    NoTlp_Supplier, 
    Alamat_Supplier
FROM Supplier;
GO

CREATE VIEW vw_KelolaKelompokTani AS
SELECT 
    Id_Kelompok, 
    Nama_Kelompok, 
    NoTlp_Kelompok, 
    Alamat_Kelompok
FROM KelompokTani;
GO

CREATE VIEW vw_Satuan AS
SELECT 
    Id_Satuan, 
    Nama_Satuan 
FROM Satuan;
GO

CREATE VIEW vw_Kategori AS
SELECT 
    Id_Kategori, 
    Nama_Kategori 
FROM Kategori;
GO

-- 3.2 Transaksi (Master & Detail, Masuk/Keluar)
CREATE VIEW vw_DaftarTransaksiIn AS
SELECT 
    t.Id_In AS [ID Transaksi], 
    s.Nama_Supplier AS [Nama Supplier], 
    t.Tgl_In AS [Tanggal Masuk], 
    t.Total_Barang_In AS [Total Kuantitas]
FROM Transaksi_In t
INNER JOIN Supplier s ON t.Id_Supplier = s.Id_Supplier;
GO

CREATE VIEW vw_DetailTransaksiIn AS
SELECT 
    d.Id_In AS [ID Transaksi],
    b.Id_Barang AS [ID Barang], 
    b.Nama_Barang AS [Nama Barang], 
    d.Subtotal_In AS [Jumlah Masuk]
FROM Detail_In d
INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang;
GO

CREATE VIEW vw_DaftarTransaksiOut AS
SELECT 
    t.Id_Out AS [ID Transaksi], 
    k.Nama_Kelompok AS [Nama Kelompok], 
    t.Tgl_Out AS [Tanggal Keluar], 
    t.Total_Barang_Out AS [Total Kuantitas]
FROM Transaksi_Out t
INNER JOIN KelompokTani k ON t.Id_Kelompok = k.Id_Kelompok;
GO

CREATE VIEW vw_DetailTransaksiOut AS
SELECT 
    d.Id_Out AS [ID Transaksi], 
    b.Id_Barang AS [ID Barang], 
    b.Nama_Barang AS [Nama Barang], 
    d.Subtotal_Out AS [Jumlah Keluar]
FROM Detail_Out d
INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang;
GO

CREATE VIEW vw_MasterTransaksiIn AS
SELECT 
    Id_In, 
    Id_Supplier, 
    Tgl_In, 
    Total_Barang_In
FROM Transaksi_In;
GO

CREATE VIEW vw_MasterTransaksiOut AS
SELECT 
    Id_Out, 
    Id_Kelompok, 
    Tgl_Out, 
    Total_Barang_Out
FROM Transaksi_Out;
GO

-- 3.3 Dropdown (untuk kebutuhan combo box di aplikasi)
CREATE VIEW vw_DropdownBarang AS
SELECT Id_Barang, Nama_Barang, Stok_Barang 
FROM Barang;
GO

CREATE VIEW vw_DropdownKategori AS
SELECT Id_Kategori, Nama_Kategori 
FROM Kategori;
GO

CREATE VIEW vw_DropdownSupplier AS
SELECT Id_Supplier, Nama_Supplier 
FROM Supplier;
GO

CREATE VIEW vw_DropdownKelompokTani AS
SELECT Id_Kelompok, Nama_Kelompok 
FROM KelompokTani;
GO


/* ============================================================================
   4. STORED PROCEDURE
   ============================================================================ */

-- 4.1 INSERT -----------------------------------------------------------------
CREATE PROCEDURE sp_InsertBarang
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @IdSatuan VARCHAR(10),
    @IdKategori VARCHAR(10),
    @Foto VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Barang WHERE Id_Barang = @Id)
    BEGIN
        RAISERROR ('Gagal! ID Barang sudah terdaftar di sistem.', 16, 1);
        RETURN;
    END

    INSERT INTO Barang (Id_Barang, Nama_Barang, Id_Satuan, Id_Kategori, Stok_Barang, Foto)
    VALUES (@Id, @Nama, @IdSatuan, @IdKategori, 0, @Foto);
END;
GO

CREATE PROCEDURE sp_InsertSupplier
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @NoTlp VARCHAR(20),
    @Alamat VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Supplier WHERE Id_Supplier = @Id)
    BEGIN
        RAISERROR ('Gagal! ID Supplier sudah terdaftar.', 16, 1);
        RETURN;
    END

    INSERT INTO Supplier (Id_Supplier, Nama_Supplier, NoTlp_Supplier, Alamat_Supplier)
    VALUES (@Id, @Nama, @NoTlp, @Alamat);
END;
GO

CREATE PROCEDURE sp_InsertKelompokTani
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @NoTlp VARCHAR(20),
    @Alamat VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM KelompokTani WHERE Id_Kelompok = @Id)
    BEGIN
        RAISERROR ('Gagal! ID Kelompok Tani sudah terdaftar.', 16, 1);
        RETURN;
    END

    INSERT INTO KelompokTani (Id_Kelompok, Nama_Kelompok, NoTlp_Kelompok, Alamat_Kelompok)
    VALUES (@Id, @Nama, @NoTlp, @Alamat);
END;
GO

CREATE PROCEDURE sp_InsertSatuan
    @Id VARCHAR(10),
    @Nama VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Satuan WHERE Id_Satuan = @Id)
    BEGIN
        RAISERROR ('Gagal! ID Satuan sudah terdaftar.', 16, 1);
        RETURN;
    END

    INSERT INTO Satuan (Id_Satuan, Nama_Satuan)
    VALUES (@Id, @Nama);
END;
GO

CREATE PROCEDURE sp_InsertKategori
    @Id VARCHAR(10),
    @Nama VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Kategori WHERE Id_Kategori = @Id)
    BEGIN
        RAISERROR ('Gagal! ID Kategori sudah terdaftar.', 16, 1);
        RETURN;
    END

    INSERT INTO Kategori (Id_Kategori, Nama_Kategori)
    VALUES (@Id, @Nama);
END;
GO

CREATE PROCEDURE sp_InsertTransaksiIn
    @Id VARCHAR(10),
    @IdSupplier VARCHAR(10),
    @Tgl DATE,
    @Total INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Transaksi_In (Id_In, Id_Supplier, Tgl_In, Total_Barang_In)
    VALUES (@Id, @IdSupplier, @Tgl, @Total);
END;
GO

CREATE PROCEDURE sp_InsertDetailIn
    @IdIn VARCHAR(10),
    @IdBarang VARCHAR(10),
    @Qty INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Detail_In (Id_In, Id_Barang, Subtotal_In)
    VALUES (@IdIn, @IdBarang, @Qty);

    UPDATE Barang 
    SET Stok_Barang = Stok_Barang + @Qty 
    WHERE Id_Barang = @IdBarang;
END;
GO

CREATE PROCEDURE sp_InsertTransaksiOut
    @Id VARCHAR(10),
    @IdKelompok VARCHAR(10),
    @Tgl DATE,
    @Total INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Transaksi_Out (Id_Out, Id_Kelompok, Tgl_Out, Total_Barang_Out)
    VALUES (@Id, @IdKelompok, @Tgl, @Total);
END;
GO

CREATE PROCEDURE sp_InsertDetailOut
    @IdOut VARCHAR(10),
    @IdBarang VARCHAR(10),
    @Qty INT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Detail_Out (Id_Out, Id_Barang, Subtotal_Out)
    VALUES (@IdOut, @IdBarang, @Qty);

    UPDATE Barang 
    SET Stok_Barang = Stok_Barang - @Qty 
    WHERE Id_Barang = @IdBarang;
END;
GO

CREATE PROCEDURE sp_InsertLogError
    @Pesan VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Log_Error (Pesan_Error) VALUES (@Pesan);
END;
GO


-- 4.2 UPDATE -----------------------------------------------------------------
CREATE PROCEDURE sp_UpdateBarang
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @IdSatuan VARCHAR(10),
    @IdKategori VARCHAR(10),
    @Foto VARBINARY(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Barang WHERE Id_Barang = @Id)
    BEGIN
        RAISERROR ('Gagal! Data barang tidak ditemukan.', 16, 1);
        RETURN;
    END

    UPDATE Barang 
    SET Nama_Barang = @Nama, 
        Id_Satuan = @IdSatuan, 
        Id_Kategori = @IdKategori,
        Foto = @Foto 
    WHERE Id_Barang = @Id;
END;
GO

CREATE PROCEDURE sp_UpdateSupplier
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @NoTlp VARCHAR(20),
    @Alamat VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Supplier 
    SET Nama_Supplier = @Nama, NoTlp_Supplier = @NoTlp, Alamat_Supplier = @Alamat 
    WHERE Id_Supplier = @Id;
END;
GO

CREATE PROCEDURE sp_UpdateKelompokTani
    @Id VARCHAR(10),
    @Nama VARCHAR(100),
    @NoTlp VARCHAR(20),
    @Alamat VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE KelompokTani 
    SET Nama_Kelompok = @Nama, NoTlp_Kelompok = @NoTlp, Alamat_Kelompok = @Alamat 
    WHERE Id_Kelompok = @Id;
END;
GO

CREATE PROCEDURE sp_UpdateSatuan
    @Id VARCHAR(10),
    @Nama VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM Satuan WHERE Id_Satuan = @Id)
    BEGIN
        RAISERROR ('Gagal! Data Satuan tidak ditemukan di sistem.', 16, 1);
        RETURN;
    END

    UPDATE Satuan 
    SET Nama_Satuan = @Nama 
    WHERE Id_Satuan = @Id;
END;
GO

CREATE PROCEDURE sp_UpdateKategori
    @Id VARCHAR(10),
    @Nama VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Kategori WHERE Id_Kategori = @Id)
    BEGIN
        RAISERROR ('Gagal! Data Kategori tidak ditemukan di sistem.', 16, 1);
        RETURN;
    END

    UPDATE Kategori 
    SET Nama_Kategori = @Nama 
    WHERE Id_Kategori = @Id;
END;
GO


-- 4.3 DELETE -----------------------------------------------------------------
CREATE PROCEDURE sp_DeleteBarang
    @Id VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StokSaatIni INT;

    SELECT @StokSaatIni = Stok_Barang FROM Barang WHERE Id_Barang = @Id;

    IF (@StokSaatIni > 0)
    BEGIN
        RAISERROR ('Gagal menghapus! Barang ini masih memiliki stok fisik di gudang.', 16, 1);
        RETURN;
    END

    DELETE FROM Barang WHERE Id_Barang = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteSupplier
    @Id VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Transaksi_In WHERE Id_Supplier = @Id)
    BEGIN
        RAISERROR ('Sistem Menolak! Supplier ini memiliki riwayat transaksi masuk dan tidak boleh dihapus.', 16, 1);
        RETURN;
    END

    DELETE FROM Supplier WHERE Id_Supplier = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteKelompokTani
    @Id VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Transaksi_Out WHERE Id_Kelompok = @Id)
    BEGIN
        RAISERROR ('Sistem Menolak! Kelompok Tani ini memiliki riwayat penerimaan barang (distribusi) dan tidak boleh dihapus.', 16, 1);
        RETURN;
    END

    DELETE FROM KelompokTani WHERE Id_Kelompok = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteSatuan
    @Id VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Barang WHERE Id_Satuan = @Id)
    BEGIN
        RAISERROR ('Sistem Menolak! Satuan ini sedang digunakan oleh data Barang dan tidak boleh dihapus.', 16, 1);
        RETURN;
    END

    DELETE FROM Satuan WHERE Id_Satuan = @Id;
END;
GO

CREATE PROCEDURE sp_DeleteKategori
    @Id VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Barang WHERE Id_Kategori = @Id)
    BEGIN
        RAISERROR ('Sistem Menolak! Kategori ini sedang digunakan oleh data Barang dan tidak boleh dihapus.', 16, 1);
        RETURN;
    END

    DELETE FROM Kategori WHERE Id_Kategori = @Id;
END;
GO


-- 4.4 GENERATE ID (pindahan dari backend) 
CREATE PROCEDURE sp_GenerateIdBarang
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Barang FROM Barang ORDER BY Id_Barang DESC;
    
    IF @LastID IS NULL SET @NewID = 'BR-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'BR-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdIn
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_In FROM Transaksi_In ORDER BY Id_In DESC;
    
    IF @LastID IS NULL SET @NewID = 'TR-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'TR-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdOut
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Out FROM Transaksi_Out ORDER BY Id_Out DESC;
    
    IF @LastID IS NULL SET @NewID = 'TR-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'TR-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdSupplier
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Supplier FROM Supplier ORDER BY Id_Supplier DESC;
    
    IF @LastID IS NULL SET @NewID = 'SP-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'SP-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdKelompok
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Kelompok FROM KelompokTani ORDER BY Id_Kelompok DESC;
    
    IF @LastID IS NULL SET @NewID = 'KL-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'KL-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdKategori
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Kategori FROM Kategori ORDER BY Id_Kategori DESC;
    
    IF @LastID IS NULL SET @NewID = 'KT-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'KT-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO

CREATE PROCEDURE sp_GenerateIdSatuan
AS
BEGIN
    DECLARE @LastID VARCHAR(10), @NewID VARCHAR(10), @Number INT;
    SELECT TOP 1 @LastID = Id_Satuan FROM Satuan ORDER BY Id_Satuan DESC;
    
    IF @LastID IS NULL SET @NewID = 'ST-001';
    ELSE BEGIN
        SET @Number = CAST(SUBSTRING(@LastID, 4, 3) AS INT) + 1;
        SET @NewID = 'ST-' + RIGHT('000' + CAST(@Number AS VARCHAR(3)), 3);
    END
    SELECT @NewID AS NewID;
END;
GO


-- 4.5 LAPORAN / LOOKUP --------------------------------------------------------
CREATE PROCEDURE sp_GetDropdownSatuan
    @NamaKategori VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @NamaKategori IS NULL OR @NamaKategori = ''
    BEGIN
        SELECT Id_Satuan, Nama_Satuan FROM Satuan WHERE 1 = 0; 
        RETURN;
    END

    IF @NamaKategori LIKE '%pupuk%' OR @NamaKategori LIKE '%bibit%' OR @NamaKategori LIKE '%benih%'
    BEGIN
        SELECT Id_Satuan, Nama_Satuan FROM Satuan 
        WHERE Nama_Satuan IN ('Kg', 'Gram', 'Sak', 'Karung', 'Ton', 'Pack');
    END
    ELSE IF @NamaKategori LIKE '%obat%' OR @NamaKategori LIKE '%pestisida%' OR @NamaKategori LIKE '%herbisida%'
    BEGIN
        SELECT Id_Satuan, Nama_Satuan FROM Satuan 
        WHERE Nama_Satuan IN ('Liter', 'Botol', 'Mililiter', 'Pack');
    END
    ELSE IF @NamaKategori LIKE '%alat%' OR @NamaKategori LIKE '%kemasan%'
    BEGIN
        SELECT Id_Satuan, Nama_Satuan FROM Satuan 
        WHERE Nama_Satuan IN ('Unit', 'Pcs', 'Box', 'Buah');
    END
    ELSE
    BEGIN
        SELECT Id_Satuan, Nama_Satuan FROM Satuan;
    END
END;
GO

CREATE PROCEDURE sp_GetStatistikaTransaksiTahunan
    @Tahun INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH BulanCTE AS (
        SELECT 1 AS BulanNum, 'Jan' AS BulanNama UNION ALL
        SELECT 2, 'Feb' UNION ALL
        SELECT 3, 'Mar' UNION ALL
        SELECT 4, 'Apr' UNION ALL
        SELECT 5, 'Mei' UNION ALL
        SELECT 6, 'Jun' UNION ALL
        SELECT 7, 'Jul' UNION ALL
        SELECT 8, 'Agu' UNION ALL
        SELECT 9, 'Sep' UNION ALL
        SELECT 10, 'Okt' UNION ALL
        SELECT 11, 'Nov' UNION ALL
        SELECT 12, 'Des'
    ),
    MasukData AS (
        SELECT 
            MONTH(Tgl_In) AS BulanNum, 
            SUM(Total_Barang_In) AS TotalMasuk
        FROM Transaksi_In
        WHERE YEAR(Tgl_In) = @Tahun
        GROUP BY MONTH(Tgl_In)
    ),
    KeluarData AS (
        SELECT 
            MONTH(Tgl_Out) AS BulanNum, 
            SUM(Total_Barang_Out) AS TotalKeluar
        FROM Transaksi_Out
        WHERE YEAR(Tgl_Out) = @Tahun
        GROUP BY MONTH(Tgl_Out)
    )
    SELECT 
        b.BulanNama AS Bulan,
        ISNULL(m.TotalMasuk, 0) AS Total_Masuk,
        ISNULL(k.TotalKeluar, 0) AS Total_Keluar
    FROM BulanCTE b
    LEFT JOIN MasukData m ON b.BulanNum = m.BulanNum
    LEFT JOIN KeluarData k ON b.BulanNum = k.BulanNum
    ORDER BY b.BulanNum;
END;
GO

CREATE PROCEDURE sp_CetakNotaIn
    @IdIn VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.Id_In AS ID_Transaksi,
        s.Nama_Supplier,
        t.Tgl_In AS Tanggal,
        b.Nama_Barang,
        d.Subtotal_In AS Jumlah
    FROM Transaksi_In t
    INNER JOIN Supplier s ON t.Id_Supplier = s.Id_Supplier
    INNER JOIN Detail_In d ON t.Id_In = d.Id_In
    INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang
    WHERE t.Id_In = @IdIn;
END;
GO

CREATE PROCEDURE sp_CetakNotaOut
    @IdOut VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        t.Id_Out AS ID_Transaksi,
        k.Nama_Kelompok,
        t.Tgl_Out AS Tanggal,
        b.Nama_Barang,
        d.Subtotal_Out AS Jumlah
    FROM Transaksi_Out t
    INNER JOIN KelompokTani k ON t.Id_Kelompok = k.Id_Kelompok
    INNER JOIN Detail_Out d ON t.Id_Out = d.Id_Out
    INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang
    WHERE t.Id_Out = @IdOut;
END;
GO


/* ============================================================================
   5. TRIGGER
   ============================================================================ */

-- Log otomatis ke Log_Aktivitas saat Barang di-INSERT atau di-UPDATE
CREATE TRIGGER trg_LogInsertUpdateBarang
ON Barang
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @Aktivitas VARCHAR(50);
    DECLARE @Detail VARCHAR(MAX);
    
    IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        SET @Aktivitas = 'UPDATE';
        SELECT @Detail = 'Mengubah data Barang ID: ' + Id_Barang + ' (' + Nama_Barang + ')' FROM inserted;
    END
    ELSE
    BEGIN
        SET @Aktivitas = 'INSERT';
        SELECT @Detail = 'Menambahkan Barang baru ID: ' + Id_Barang + ' (' + Nama_Barang + ')' FROM inserted;
    END

    IF @Detail IS NOT NULL
    BEGIN
        INSERT INTO Log_Aktivitas (Aktivitas, Tabel_Terkait, Detail) 
        VALUES (@Aktivitas, 'Barang', @Detail);
    END
END;
GO

-- Trigger Log Hapus Barang
CREATE TRIGGER trg_LogDeleteBarang
ON Barang
AFTER DELETE
AS
BEGIN
    DECLARE @Detail VARCHAR(MAX);
    SELECT @Detail = 'Menghapus Barang ID: ' + Id_Barang + ' (' + Nama_Barang + ')' FROM deleted;
    
    IF @Detail IS NOT NULL
        INSERT INTO Log_Aktivitas (Aktivitas, Tabel_Terkait, Detail) 
        VALUES ('DELETE', 'Barang', @Detail);
END;
GO

-- Trigger Log Hapus Supplier
CREATE TRIGGER trg_LogDeleteSupplier
ON Supplier
AFTER DELETE
AS
BEGIN
    DECLARE @Detail VARCHAR(MAX);
    SELECT @Detail = 'Menghapus Supplier ID: ' + Id_Supplier + ' (' + Nama_Supplier + ')' FROM deleted;
    
    IF @Detail IS NOT NULL
        INSERT INTO Log_Aktivitas (Aktivitas, Tabel_Terkait, Detail) 
        VALUES ('DELETE', 'Supplier', @Detail);
END;
GO

-- Trigger Log Hapus Kelompok Tani
CREATE TRIGGER trg_LogDeleteKelompok
ON KelompokTani
AFTER DELETE
AS
BEGIN
    DECLARE @Detail VARCHAR(MAX);
    SELECT @Detail = 'Menghapus Kelompok Tani ID: ' + Id_Kelompok + ' (' + Nama_Kelompok + ')' FROM deleted;
    
    IF @Detail IS NOT NULL
        INSERT INTO Log_Aktivitas (Aktivitas, Tabel_Terkait, Detail) 
        VALUES ('DELETE', 'KelompokTani', @Detail);
END;
GO
