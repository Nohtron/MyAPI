using Microsoft.AspNetCore.Mvc;
using MyAPI.Models;
using IronXL;
using System.Data;

namespace MyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CoilController : ControllerBase
{
    private IEnumerable<Coil> GetAllCoils()
    {
        //Supported spreadsheet formats for reading include: XLSX, XLS, CSV and TSV
        WorkBook workbook = WorkBook.Load("D:\\Repos\\Temp\\MyAPI\\Data\\DEC -XCR - 04_09_2020 - 10dias.xls");
        WorkSheet sheet = workbook.WorkSheets.First();

        //Convert the worksheet to System.Data.DataTable
        //Boolean parameter sets the first row as column names of your table.
        DataTable dataTable = sheet.ToDataTable(true);

        //The DataTable can populate a DataGrid for example.

        List<Coil> coils = new List<Coil>();

        int id = 0;
        double weight = 0.0;
        double thickness = 0.0;
        double width = 0.0;
        double length = 0.0;

        //Enumerate by rows or columns first at your preference
        foreach (DataRow row in dataTable.Rows)
        {
            bool idParse = int.TryParse(row["IDC_XCOIL_REP"].ToString(), out id); 
            bool weightParse = double.TryParse(row["WG_ME_COIL"].ToString(), out weight);
            bool thicknessParse = double.TryParse(row["TH_ME_STRIP"].ToString(), out thickness);
            bool lengthParse = double.TryParse(row["LG_XCOIL"].ToString(), out length);
            bool widthParse = double.TryParse(row["WD_XCOIL"].ToString(), out width);

            if (idParse && weightParse && thicknessParse && lengthParse && widthParse)
            {
                Coil coil = new Coil()
                {
                    Id = id,
                    Name = row["COIL_ID"].ToString(),
                    Weight = weight,
                    Thickness = thickness,
                    Length = length,
                    Width = width
                };
                coils.Add(coil);
            }

        }

        return coils;
    }

    [HttpGet]
    public string VerifyAndReport(string name)
    {
        Coil? coil = GetAllCoils().Where(x => x.CheckWeight() != 0).FirstOrDefault();

        if (coil != null)
        {
            switch (coil.CheckWeight())
            {
                case 1:
                    return "Peso acima do limite tolerado. Excedendo " + (coil.GetMaxWeightTolerated() - coil.Weight).ToString("F2") + " Kg.";
                case -1:
                    return "Peso abaixo do limite tolerado. Faltando " + (coil.GetMinWeightTolerated() - coil.Weight).ToString("F2") + " Kg.";
                case 0:
                    return "Peso dentro dos limites tolerados.";
                default:
                    return "Validação de peso não realizada!";
            }
        }
        else
        {
            throw new ArgumentException("Bobina " + name + " não encontrada!");
        }
    }

}
