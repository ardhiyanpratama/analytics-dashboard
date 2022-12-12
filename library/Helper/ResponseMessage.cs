using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library.Helper
{
	public class ResponseMessage
	{
		public string Header { get; set; }
		public string Detail { get; set; }
		public string Note { get; set; } = "";
		public dynamic? Data { get; set; }
	}

	public static class ResponseMessageExtensions
	{
		public static class Database
		{
			public const string DATA_NOT_FOUND = "Data Tidak Ditemukan";
			public const string DATA_NOT_VALID = "Pastikan Data Sudah Lengkap Dan Benar";
			public const string DATA_ALREADY_EXIST = "Data Duplikat";
			public const string DELETE_SUCCESS = "Data Berhasil Dihapus";
			public const string DELETE_FAILURE = "Data Gagal Dihapus";
			public const string UPDATE_FAILURE = "Data Gagal Diubah";
			public const string UPDATE_SUCCESS = "Data Berhasil Diubah";
			public const string WRITE_SUCCESS = "Data Berhasil Ditambah";
			public const string SAVE_SUCCESS = "Data Berhasil Disimpan";
			public const string PROC_SUCCESS = "Berhasil Menjalankan Prosedur";
			public const string DBERROR_LOG = "Tidak Ada Error di Logs";
		}

		public static class File
		{
			public const string DEFAULT_ERROR = "File Error";
			public const string NOT_FOUND = "File Tidak Ditemukan";
			public const string UPLOAD_SUCCESS = "Berhasil Upload File";
			public const string UPLOAD_FAILED = "Gagal Upload File";
			public const string EXTENSIONS_NOT_ALLOWED = "File Tidak Diperbolehkan Untuk Di Upload";
			public const string SIZE_OVER_LIMIT = "Ukuran File Melebihi Maksimal Yang Di Perbolehkan";
			public const string SIGNATURE_INVALID = "Signature File Error";
			public const string SIZE_NOT_VALID = "File Tidak Bisa Diproses";
			public const string CREATION_ERROR = "Tidak Dapat Menyimpan File";
			public const string THUMB_CREATION_ERROR = "Tidak Dapat Membuat Thumbnail";
			public const string DELETE_SUCCESS = "Berhasil Menghapus File";
			public const string DELETE_FAILED = "Gagal Menghapus File";
		}

		public static class Access
		{
			public const string UNAUTHORIZED = "Unauthorized";
			public const string FEATURE_DISABLED = "Fitur Di Non Aktifkan";
			public const string RESOURCE_CONFLICT = "Resource ini bukan milik anda";
		}

		public const string PROHIBITED_EDIT_DEFAULT = "Tidak Bisa Edit Kode DONE dan DEFAULT";
		public const string AGENT_CANT_SAME = "Tipe Agen Tidak Boleh Sama";
		public const string CODE_TYPE_VEHICLE = "Kode Jenis Kendaraan Tidak Boleh Sama";
		public const string USERNAME_ALREADY_EXIST = "Username Sudah Dipakai";
		public const string USER_INACTIVE = "User Tidak Ada/Tidak Aktif";
		public const string WRONG_PASSWORD = "Kata Sandi Salah";
		public const string EMAIL_NOT_FOUND = "Email tidak ditemukan";
		public const string CHECK_YOUR_EMAIL = "Silakan cek email Anda";
		public const string EXPIRED_CHECK_PASSWORD = "Tautan kadaluarsa, silakan lupa password ulang";
		public const string PASSWORD_HAS_CHANGED = "Kata sandi telah berhasil diubah";
		public const string VOUCHER_SUCCESS = "Voucher berhasil dibuat";
		public const string VOUCHER_CODE_NOTFOUND = "Kode voucher tidak ditemukan/tidak berlaku";
		public const string DIFFERENT_LABEL_AMOUNT = "Jumlah Label Tidak Sama Dengan Jumlah Barang";
		public const string RECEIPT_NUMBER_ERROR = "Nomor Resi Tidak Terbentuk Silahkan Coba Lagi";
		public const string LABEL_NOT_FOUND = "Label Tidak Ditemukan";
		public const string STICKER_NOT_FOUND = "Sticker Tidak Ditemukan";
		public const string NOT_ENOUGH_STICKER = "Jumlah Stiker Kurang";
		public const string PRICE_NOT_FOUND = "Parent Harga Tidak Ditemukan";
		public const string SCHEDULE_NOT_FOUND = "Data Asal Jadwal Tidak Ditemukan";
		public const string AGENT_EXIST = "Agen Sudah Terdaftar Sebelumnya";
		public const string SCHEDULE_CODE_CANT_SAME = "Kode Jadwal tidak boleh sama untuk tanggal yang sama";
		public const string DIFFERENT_SCHEDULE_ID = "Id Jadwal Berbeda";
		public const string PENDING_RECEIPT = "Resi Masuk Ke Dalam List Pending";
		public const string PACKET_RECEIVED = "Paket Sudah Diserah Terima";
		public const string PACKET_FAILED = "Tidak Dapat Melakukan Penerimaan";
		public const string UPLOAD_FAILED = "Upload Gagal ";
		public const string UPLOAD_SUCCESS = "Upload Berhasil ";
		public const string RESTRICT_DELETE_FILE = "Anda Tidak Dapat Menghapus File Ini";
		public const string INSURANCE_SAME = "Asuransi Harus Sama";
		public const string DELIVERY_FAILED = "Ada Pengiriman Yang Belum Bisa Diterima";
		public const string SCHEDULE_CLOSED = "Jadwal Sudah Ditutup";
		public const string LABEL_NUMBERING_CREATED = "Nomor Label Berhasil Dibuat";
		public const string ASSIGN_COURIER_SUCCESS = "Assign Kurir Berhasil";
		public const string TRANSFER_KURIR_SUCCESS = "Transfer Kurir Berhasil";
		public const string SUCCESS_SCAN_PACKAGE = "Paket Telah Berhasil Diambil";
		public const string FAILED_SCAN_PACKAGE = "Paket Gagal Diambil";
		public const string ADJUSTMENT_SUCCESS = "Adjustment Berhasil Ditambahkan";
		public const string BAD_PHONE_NUMBER_FORMAT = "Nomor Hp Tidak Valid";

		public const string SUCCESS_HEADER = "Sukses!";
		public const string FAIL_HEADER = "Gagal!";
		public const string DEFAULT_DETAIL_MESSAGE = "Hubungi Administrator";
		public const string HAS_NO_ACCESS = "Anda Tidak Mempunyai Akses";

		private static class ContentType
		{
			public const string ApplicationJson = "application/json";
		}

		public static ObjectResult InternalServerError(this ControllerBase controller, string message = null)
		{
			return controller.StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = message ?? DEFAULT_DETAIL_MESSAGE
			});
		}

		public static OkObjectResult OkResponse(this ControllerBase controller, string message, string? note = null, dynamic? data = null)
		{
			return controller.Ok(new ResponseMessage
			{
				Header = SUCCESS_HEADER,
				Detail = message,
				Note = note ?? "",
				Data = data ?? null
			});
		}

		public static ObjectResult AcceptedResult(this ControllerBase controller, string message, string? note = null, dynamic? data = null)
		{
			return controller.Accepted(new ResponseMessage
			{
				Header = SUCCESS_HEADER,
				Detail = message,
				Note = note ?? "",
				Data = data ?? null
			});
		}

		public static async Task BadRequestResponse(this HttpResponse response, string message, string? note = null, dynamic? data = null)
		{
			response.ContentType = ContentType.ApplicationJson;
			response.StatusCode = StatusCodes.Status400BadRequest;
			var responseMessage = new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = message,
				Note = note,
				Data = data
			};

			await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
		}

		public static async Task InternalErrorResponse(this HttpResponse response, string message)
		{
			response.ContentType = ContentType.ApplicationJson;
			response.StatusCode = StatusCodes.Status500InternalServerError;
			var responseMessage = new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = DEFAULT_DETAIL_MESSAGE,
				Note = message
			};

			await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
		}

		public static async Task UnathourizedResponse(this HttpResponse response, string message)
		{
			response.ContentType = ContentType.ApplicationJson;
			response.StatusCode = StatusCodes.Status401Unauthorized;
			var responseMessage = new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = HAS_NO_ACCESS,
				Note = message
			};

			await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
		}

		public static async Task ServiceUnavailableResponse(this HttpResponse response, string message)
		{
			response.ContentType = ContentType.ApplicationJson;
			response.StatusCode = StatusCodes.Status503ServiceUnavailable;
			var responseMessage = new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = message
			};

			await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
		}

		public static async Task ForbiddedResponse(this HttpResponse response)
		{
			response.ContentType = ContentType.ApplicationJson;
			response.StatusCode = StatusCodes.Status403Forbidden;
			var responseMessage = new ResponseMessage
			{
				Header = FAIL_HEADER,
				Detail = "Anda Tidak Mempunyai Akses Terhadap Resource Ini"
			};

			await response.WriteAsync(JsonSerializer.Serialize(responseMessage));
		}
	}

}

