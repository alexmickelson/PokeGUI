using OfficeOpenXml;
using PokeGUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PokeGUI.Services
{
    public class PokemonExcelService : IPokemonExcelService
    {
        public FileInfo filterFileInfo { get; set; }
        public string pokeNameFilter { get; set; }
        public PokeType pokeTypeFilter { get; set; }
        public (string, PokeType) getStoredFilter()
        {
            selectFilterFile();
            readValuesFromFilterFile();

            return (pokeNameFilter, pokeTypeFilter);
        }

        private void readValuesFromFilterFile()
        {
            pokeNameFilter = string.Empty;
            pokeTypeFilter = new PokeType("none");

            using (var filterPackage = new ExcelPackage(filterFileInfo))
            {
                var worksheet = filterPackage.Workbook.Worksheets[0];
                pokeNameFilter = worksheet.Cells["B1"].Value.ToString();
                pokeTypeFilter = new PokeType(worksheet.Cells["B2"].Value.ToString());
            }
        }

        private void selectFilterFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel |*.xl*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filterFileInfo = new FileInfo(openFileDialog.FileName);
            }
        }

        public void generatePokemonExcelSheet(IEnumerable<Pokemon> pokemonCollection)
        {

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ZRAM";
                excelPackage.Workbook.Properties.Title = "Your Poke Dex";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                var worksheet = excelPackage.Workbook.Worksheets.Add("Pokemon");

                worksheet.Cells[1, 1].Value = "Your Poke Dex";

                worksheet.Cells[2, 1].Value = "ID";
                worksheet.Cells[2, 2].Value = "Name";
                worksheet.Cells[2, 3].Value = "Type 1";
                worksheet.Cells[2, 4].Value = "Type 2";
                worksheet.Cells[2, 5].Value = "Image URL";

                var rowIndex = 3;
                foreach (var pokemon in pokemonCollection)
                {
                    worksheet.Cells[rowIndex, 1].Value = pokemon.PokeId;
                    worksheet.Cells[rowIndex, 2].Value = pokemon.Name;
                    worksheet.Cells[rowIndex, 3].Value = pokemon.Type1;
                    worksheet.Cells[rowIndex, 4].Value = pokemon.Type2;
                    worksheet.Cells[rowIndex, 5].Value = pokemon.Image;
                    rowIndex++;
                }

                FileInfo saveFile = new FileInfo(@"C:\Users\Zachary Reiss\Documents\School\pokeDex.xlsx");
                excelPackage.SaveAs(saveFile);

                Process process = new Process();
                process.StartInfo.FileName = saveFile.FullName;
                process.StartInfo.UseShellExecute = true;
                process.Start();

            }
        }
    }
}
