using System.Data;
using OfficeOpenXml;

namespace BTL_ASP_MVC.Models.Process
{
  public class ExcelProcess
  {
    public DataTable ExcelToDataTable(string strPath)
    {
      FileInfo fi = new FileInfo(strPath);
      ExcelPackage excelPackage = new ExcelPackage(fi);
      DataTable dt = new DataTable();
      ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

      if (worksheet.Dimension == null)
      {
        return dt;
      }

      List<string> columnNames = new List<string>();
      int currentColumn = 1;
      foreach (var cell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
      {
        string columnName = cell.Text.Trim();

        //check if the previous header was empty and add it if it was
        if (cell.Start.Column != currentColumn)
        {
          columnNames.Add("Header_" + currentColumn);
          dt.Columns.Add("Header_" + currentColumn);
          currentColumn++;
        }

        //add the column name to the list to count the duplicates
        columnNames.Add(columnName);

        //count the duplicate column names and make them unique to avoid the exception
        //A column named 'Name' already belongs to this DataTable
        int occurrences = columnNames.Count(x => x.Equals(columnName));
        if (occurrences > 1)
        {
          columnName = columnName + "_" + occurrences;
        }

        //add the column to the datatable
        dt.Columns.Add(columnName);

        currentColumn++;
      }

      for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
      {
        var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
        DataRow newRow = dt.NewRow();

        //loop all cells in the row
        foreach (var cell in row)
        {
          newRow[cell.Start.Column - 1] = cell.Text;
        }

        dt.Rows.Add(newRow);
      }

      return dt;
    }
  }
}