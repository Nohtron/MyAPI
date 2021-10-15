using Microsoft.AspNetCore.Mvc;
using MyAPI.Models;
using IronXL;
using System.Data;

namespace MyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CoilController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Coil> Get() //IEnumerable<Coil> Get()
    {
        //Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
        WorkBook workbook = WorkBook.Load("D:\\Repos\\Temp\\MyAPI\\Data\\DEC -XCR - 04_09_2020 - 10dias.xls");
        WorkSheet sheet = workbook.WorkSheets.First();

        //Convert the worksheet to System.Data.DataTable
        //Boolean parameter sets the first row as column names of your table.
        DataTable dataTable = sheet.ToDataTable(true);
        
        //The DataTable can populate a DataGrid for example.

        List<Coil> coils = new List<Coil>();

        //Enumerate by rows or columns first at your preference
        foreach (DataRow row in dataTable.Rows)
        {

            System.Console.WriteLine("IDC: " + row["IDC_XCOIL_REP"].ToString());
            
            Coil coil = new Coil()
            {
                Id = int.Parse(row["IDC_XCOIL_REP"].ToString()),
                Name = row["COIL_ID"].ToString(),
                Weight = double.Parse(row["WG_ME_COIL"].ToString()),
                Thickness = double.Parse(row["TH_ME_STRIP"].ToString()),
                Length = double.Parse(row["LG_XCOIL"].ToString()),
                Width = double.Parse(row["WD_XCOIL"].ToString())                
            };
            
            coils.Add(coil);
        }

        return coils;
    }
    
}
