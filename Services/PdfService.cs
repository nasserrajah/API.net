using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using core_first.API.Services;
using core_first.API.Models;

namespace core_first.API.Services
{
    public class PdfService
    {
        // تقرير الأرباح والخسائر
        public byte[] GenerateProfitLossPdf(ProfitLossReport report)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().AlignCenter().Text("تقرير الأرباح والخسائر").SemiBold().FontSize(20);
                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Text($"الفترة: {report.StartDate:dd/MM/yyyy} - {report.EndDate:dd/MM/yyyy}");
                        col.Item().LineHorizontal(0.5f);
                        col.Item().Text($"إجمالي الإيرادات: {report.TotalIncome:C}");
                        col.Item().Text($"إجمالي المصروفات: {report.TotalExpense:C}");
                        col.Item().Text($"صافي الربح: {report.NetProfit:C}");
                    });
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("تم الإنشاء بواسطة نظام core_first - ");
                        x.CurrentPageNumber();
                    });
                });
            });
            return document.GeneratePdf();
        }

        // تقرير العملاء
        public byte[] GenerateCustomersPdf(List<Customer> customers)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().AlignCenter().Text("تقرير العملاء").SemiBold().FontSize(20);
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("الاسم").Bold();
                            header.Cell().Text("الهاتف").Bold();
                            header.Cell().Text("البريد الإلكتروني").Bold();
                            header.Cell().Text("العنوان").Bold();
                            header.Cell().Text("عدد المعاملات").Bold();
                        });
                        foreach (var c in customers)
                        {
                            table.Cell().Text(c.Name);
                            table.Cell().Text(c.Phone);
                            table.Cell().Text(c.Email);
                            table.Cell().Text(c.Address);
                            table.Cell().Text(c.Transactions.Count.ToString());
                        }
                    });
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("تم الإنشاء بواسطة نظام core_first - ");
                        x.CurrentPageNumber();
                    });
                });
            });
            return document.GeneratePdf();
        }

        // فاتورة PDF
        public byte[] GenerateInvoicePdf(Invoice invoice)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().AlignCenter().Text("فاتورة").SemiBold().FontSize(20);
                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Item().Text($"رقم الفاتورة: {invoice.InvoiceNumber}");
                        col.Item().Text($"تاريخ الإصدار: {invoice.IssueDate:dd/MM/yyyy}");
                        col.Item().Text($"تاريخ الاستحقاق: {invoice.DueDate:dd/MM/yyyy}");
                        col.Item().Text($"العميل/المورد: {invoice.Customer?.Name ?? invoice.Supplier?.Name ?? "غير محدد"}");
                        col.Item().LineHorizontal(0.5f);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });
                            table.Header(header =>
                            {
                                header.Cell().Text("الوصف").Bold();
                                header.Cell().Text("الكمية").Bold();
                                header.Cell().Text("سعر الوحدة").Bold();
                                header.Cell().Text("الإجمالي").Bold();
                            });
                            foreach (var item in invoice.Items)
                            {
                                table.Cell().Text(item.Description);
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text($"{item.UnitPrice:C}");
                                table.Cell().Text($"{item.Total:C}");
                            }
                        });
                        col.Item().LineHorizontal(0.5f);
                        col.Item().Text($"الإجمالي الكلي: {invoice.TotalAmount:C}").Bold();
                        col.Item().Text($"الحالة: {(invoice.Status == InvoiceStatus.Paid ? "مدفوعة" : "غير مدفوعة")}");
                    });
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("شكراً للتعامل معنا - ");
                        x.CurrentPageNumber();
                    });
                });
            });
            return document.GeneratePdf();
        }
    }
}