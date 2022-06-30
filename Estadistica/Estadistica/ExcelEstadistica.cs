using System;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

namespace Estadistica{

    #region ExcelOpenXML
    public static class ExcelXML {


        /// <summary>
        /// create the default excel formats.  These formats are required for the excel in order for it to render
        /// correctly.
        /// </summary>
        /// <returns></returns>
        static private Stylesheet CreateDefaultStylesheet() {

            Stylesheet ss = new Stylesheet();

            Fonts fts = new Fonts();
            DocumentFormat.OpenXml.Spreadsheet.Font ft = new DocumentFormat.OpenXml.Spreadsheet.Font();
            FontName ftn = new FontName();
            ftn.Val = "Calibri";
            FontSize ftsz = new FontSize();
            ftsz.Val = 10;
            ft.FontName = ftn;
            ft.FontSize = ftsz;
            fts.Append(ft);
            fts.Count = (uint)fts.ChildElements.Count;

            Fills fills = new Fills();
            Fill fill;
            PatternFill patternFill;

            //default fills used by Excel, don't changes these

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.None;
            fill.PatternFill = patternFill;
            fills.AppendChild(fill);

            fill = new Fill();
            patternFill = new PatternFill();
            patternFill.PatternType = PatternValues.Gray125;
            fill.PatternFill = patternFill;
            fills.AppendChild(fill);



            fills.Count = (uint)fills.ChildElements.Count;

            Borders borders = new Borders();
            Border border = new Border();
            border.LeftBorder = new LeftBorder();
            border.RightBorder = new RightBorder();
            border.TopBorder = new TopBorder();
            border.BottomBorder = new BottomBorder();
            border.DiagonalBorder = new DiagonalBorder();
            borders.Append(border);
            borders.Count = (uint)borders.ChildElements.Count;

            CellStyleFormats csfs = new CellStyleFormats();
            CellFormat cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            csfs.Append(cf);
            csfs.Count = (uint)csfs.ChildElements.Count;


            CellFormats cfs = new CellFormats();

            cf = new CellFormat();
            cf.NumberFormatId = 0;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);



            var nfs = new NumberingFormats();



            nfs.Count = (uint)nfs.ChildElements.Count;
            cfs.Count = (uint)cfs.ChildElements.Count;

            ss.Append(nfs);
            ss.Append(fts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(csfs);
            ss.Append(cfs);

            CellStyles css = new CellStyles(
                new CellStyle() {
                    Name = "Normal",
                    FormatId = 0,
                    BuiltinId = 0,
                }
                );

            css.Count = (uint)css.ChildElements.Count;
            ss.Append(css);

            DifferentialFormats dfs = new DifferentialFormats();
            dfs.Count = 0;
            ss.Append(dfs);

            TableStyles tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = "TableStyleMedium9";
            tss.DefaultPivotStyle = "PivotStyleLight16";
            ss.Append(tss);
            return ss;
        }
        static private void SaveCustomStylesheet(WorkbookPart workbookPart) {

            //get a copy of the default excel style sheet then add additional styles to it
            Stylesheet stylesheet = CreateDefaultStylesheet();
            CellFormats cfs = stylesheet.CellFormats;//this should already contain a default StyleIndex of 0
            NumberingFormats nfs = stylesheet.NumberingFormats;
            uint iExcelIndex = 165; //number less than 164 is reserved by excel for default formats
            NumberingFormat nf;
            CellFormat cf;

            
            // Header bold
            Font font1 = new Font();         
            Bold bold = new Bold();
            font1.Append(bold);            
            stylesheet.Fonts.Append(font1);
            stylesheet.Fonts.Count = (uint)stylesheet.Fonts.ChildElements.Count;

            cf = new CellFormat();
            cf.FontId = 1;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cfs.Append(cf);


            // Date
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = @"dd/MM/yyyy";
            nfs.Append(nf);
            nfs.Count = (uint)nfs.ChildElements.Count;

            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);


            // Hour
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = @"hh:mm:ss";
            nfs.Append(nf);
            nfs.Count = (uint)nfs.ChildElements.Count;

            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);


            // Money
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = @"$#,##0.00";
            nfs.Append(nf);
            nfs.Count = (uint)nfs.ChildElements.Count;

            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);


            // Number
            nf = new NumberingFormat();
            nf.NumberFormatId = iExcelIndex++;
            nf.FormatCode = @"#,##0";
            nfs.Append(nf);
            nfs.Count = (uint)nfs.ChildElements.Count;

            cf = new CellFormat();
            cf.NumberFormatId = nf.NumberFormatId;
            cf.FontId = 0;
            cf.FillId = 0;
            cf.BorderId = 0;
            cf.FormatId = 0;
            cf.ApplyNumberFormat = true;
            cfs.Append(cf);
            

            WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet style = workbookStylesPart.Stylesheet = stylesheet;
            style.Save();
        }


        /// <summary>
        /// CellValues = Boolean -> expects cellValue "True" or "False"
        /// CellValues = InlineString -> stores string within sheet
        /// CellValues = SharedString -> stores index within sheet. If this is called, please call CreateShareStringPart after creating all sheet data to create the shared string part
        /// CellValues = Date -> expects ((DateTime)value).ToOADate().ToString(CultureInfo.InvariantCulture) as cellValue 
        ///              and new OpenXmlAttribute[] { new OpenXmlAttribute("s", null, "1") }.ToList() as attributes so that the correct formatting can be applied
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="cellValue"></param>
        /// <param name="dataType"></param>
        /// <param name="attributes"></param>
        static private void WriteCellValueSax(OpenXmlWriter writer, object cellValue, string dataType) {

            switch ( dataType ) {

                case "Header":                
                    writer.WriteStartElement(new Cell() { DataType = CellValues.String, StyleIndex = 1 });
                    writer.WriteElement(new CellValue(cellValue.ToString()));
                    writer.WriteEndElement();
                break;

                case "DateTime":
                    writer.WriteStartElement(new Cell() { DataType = CellValues.Number, StyleIndex = 2 });
                    if ( cellValue.ToString() == "")
                        writer.WriteElement(new CellValue());
                    else
                        writer.WriteElement(new CellValue(( (DateTime)cellValue ).ToOADate().ToString()));
                    writer.WriteEndElement();
                break;

                case "TimeSpan":
                    writer.WriteStartElement(new Cell() { DataType = CellValues.Number, StyleIndex = 3 });
                    if ( cellValue.ToString() == "" )
                        writer.WriteElement(new CellValue());
                    else
                        writer.WriteElement(new CellValue(new DateTime(( (TimeSpan)cellValue ).Ticks).ToOADate().ToString()));
                    writer.WriteEndElement();
                break;

                case "Decimal":
                    writer.WriteStartElement(new Cell() { DataType = CellValues.Number, StyleIndex = 4 });
                    writer.WriteElement( new CellValue(cellValue.ToString()) );
                    writer.WriteEndElement();
                break;

                case "Byte":
                case "Int16":
                case "Int32":
                    writer.WriteStartElement(new Cell() { DataType = CellValues.Number, StyleIndex = 5 });
                    writer.WriteElement( new CellValue(cellValue.ToString()) );
                    writer.WriteEndElement();
                break;

                case "Boolean":
                    writer.WriteStartElement(new Cell() { DataType = CellValues.Boolean });
                    if ( cellValue.ToString() == "" )
                        writer.WriteElement(new CellValue());
                    else
                        writer.WriteElement( new CellValue((bool)cellValue ? "1" : "0") );
                    writer.WriteEndElement();
                break;

                default:
                    writer.WriteStartElement(new Cell() { DataType = CellValues.String});      
                    //writer.WriteElement( new CellValue(cellValue.ToString().Replace("\u001a", "")) );
                    writer.WriteElement( new CellValue(cellValue.ToString().CleanString()) );
                    writer.WriteEndElement();
                break;

            }

        }

        /// <summary>
        /// Exporta tabla a un libro de Excel y en caso de error devuelve el mensaje.
        /// </summary>        
        /// <param name="Tabla">Tabla a exportar.</param>
        /// <param name="Ruta">Ruta y nombre con el que se guardará.</param>
        /// <param name="Dispose">Indica si la tabla se va a destruir para liberar memoria.</param>
        static public string ExportToExcelSAX(ref DataTable Tabla, string Ruta, bool Dispose = true) {

            try {
                using ( SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(Ruta, SpreadsheetDocumentType.Workbook) ) {
                    // create the workbook
                    WorkbookPart workbookPart = spreadSheet.AddWorkbookPart();

                    SaveCustomStylesheet(workbookPart);

                    workbookPart.Workbook = new Workbook();
                    Sheets sheets = workbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    // create worksheet 1
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = Tabla.TableName == "" ? "Pestaña 1" : Tabla.TableName };
                    sheets.Append(sheet);


                    using ( OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart) ) {

                        writer.WriteStartElement(new Worksheet());
                        writer.WriteStartElement(new SheetData());

                        //Create header 
                        writer.WriteStartElement(new Row());
                        for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ )
                            WriteCellValueSax(writer, Tabla.Columns[iCol].ColumnName, "Header");
                        writer.WriteEndElement();


                        // Rows
                        for ( int iFila = 0; iFila < Tabla.Rows.Count; iFila++ ) {
                            writer.WriteStartElement(new Row());
                            for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ )
                                WriteCellValueSax(writer, Tabla.Rows[iFila][iCol], Tabla.Columns[iCol].DataType.Name);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement(); //end of SheetData
                        writer.WriteEndElement(); //end of worksheet
                        writer.Close();
                    }

                    //CreateShareStringPart(workbookPart);
                }

            } catch ( Exception ex ) {
                CleanMemory();
                return ex.Message;
            }

            if ( Dispose )
                Tabla.Dispose();

            CleanMemory();

            // Abre libro
            try {
                System.Diagnostics.Process.Start(Ruta);
            } catch ( Exception ) {

            }

            return "";
        }
        /// <summary>
        /// Exporta las tablas de un DataSet a un libro de Excel y en caso de error devuelve el mensaje.
        /// </summary>
        /// <param name="Tablas">Data set con tablas a exportar.</param>
        /// <param name="Ruta">Ruta y nombre con el que se guardará.</param>
        /// <param name="Dispose">Indica si la tabla se va a destruir para liberar memoria.</param>
        static public string ExportToExcelSAX(ref DataSet Tablas, string Ruta, bool Dispose = true) {

            try {
                using ( SpreadsheetDocument spreadSheet = SpreadsheetDocument.Create(Ruta, SpreadsheetDocumentType.Workbook) ) {
                    // create the workbook
                    WorkbookPart workbookPart = spreadSheet.AddWorkbookPart();

                    SaveCustomStylesheet(workbookPart);

                    workbookPart.Workbook = new Workbook();
                    Sheets sheets = workbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    uint iHojas = 1;
                    foreach ( DataTable Tabla in Tablas.Tables ) {

                        // create worksheet 1
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = iHojas++, Name = Tabla.TableName == "" ? "Pestaña " + iHojas : Tabla.TableName };
                        sheets.Append(sheet);

                        using ( OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart) ) {

                            writer.WriteStartElement(new Worksheet());
                            writer.WriteStartElement(new SheetData());

                            //Create header 
                            writer.WriteStartElement(new Row());
                            for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ )
                                WriteCellValueSax(writer, Tabla.Columns[iCol].ColumnName, "Header");
                            writer.WriteEndElement();


                            // Rows
                            for ( int iFila = 0; iFila < Tabla.Rows.Count; iFila++ ) {
                                writer.WriteStartElement(new Row());
                                for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ )
                                    WriteCellValueSax(writer, Tabla.Rows[iFila][iCol], Tabla.Columns[iCol].DataType.Name);
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement(); //end of SheetData
                            writer.WriteEndElement(); //end of worksheet
                            writer.Close();
                        }
                    }
                }

            } catch ( Exception ex ) {
                CleanMemory();
                return ex.Message;
            }

            if ( Dispose )
                Tablas.Dispose();

            CleanMemory();

            // Abre libro
            try {
                System.Diagnostics.Process.Start(Ruta);
            } catch ( Exception ) {

            }

            return "";
        }


        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        static bool IsLegalXmlChar(int character) {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                ( character >= 0x20 && character <= 0xD7FF ) ||
                ( character >= 0xE000 && character <= 0xFFFD ) ||
                ( character >= 0x10000 && character <= 0x10FFFF )
            );
        }
        /// <summary>
        /// Cleans string from ilegal Xml characters.
        /// </summary>
        /// <param name="Texto">String to clean</param>
        static public string CleanString(this string Texto) { 
            string sNuevo = "";
            foreach ( char caracter in Texto )
                if ( IsLegalXmlChar(caracter) )
                    sNuevo += caracter;
            return sNuevo;
        }


        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize")]
        private static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        private static void CleanMemory() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            int iMinMB = 15 * ( 1024 ^ 3 );
            int iMaxMB = 230 * ( 1024 ^ 3 );

            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, iMinMB, iMaxMB);
        }

    }

    #endregion

    #region ExcelInterop
    /*
    public class ExcelInterop {

        public static string CreateBook(string filePath, DataTable Tabla) {
            string sResultado = "";
            
            try {
                Microsoft.Office.Interop.Excel.Application xlsPruebaVentana = null;                
            } catch ( Exception ex ) {
                return "Necesita tener instalado Microsoft Excel.\r\n" + ex.Message;
            }
            Microsoft.Office.Interop.Excel.Application xlsVentana  = new Microsoft.Office.Interop.Excel.Application();

            try {
                xlsVentana.Visible = false;
            } catch ( Exception ex ) {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsVentana);
                xlsVentana = null;
                return "La version de Microsoft Excel tuvo problemas.\r\n" + ex.Message;
            }

            Microsoft.Office.Interop.Excel._Workbook xlsLibro = null;
            try {
                xlsLibro = (Microsoft.Office.Interop.Excel._Workbook)xlsVentana.Workbooks.Add(System.Reflection.Missing.Value);
            } catch ( Exception ex ) {
                xlsVentana.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsVentana);
                xlsVentana = null;
                return "Fallo al crear libro de Excel. \r\n" + ex.Message;
            }

            Microsoft.Office.Interop.Excel._Worksheet xlsHoja = null;
            try {
                xlsHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlsLibro.ActiveSheet;
            } catch ( Exception ex ) {
                xlsLibro.Close(false, "", 0);
                xlsVentana.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsVentana);
                xlsVentana = null;
                return "Fallo al crear hoja de Excel.\r\n" + ex.Message;
            }

            xlsHoja.Name = Tabla.TableName.Trim() == "" ? "Pestaña 1" : Tabla.TableName;

            ((Microsoft.Office.Interop.Excel.Range)xlsHoja.Cells[1, 1]).EntireRow.Font.Bold = true;

            string sColumnFormat = "@";
            for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ ) {
                xlsHoja.Cells[1, iCol + 1] = Tabla.Columns[iCol].ColumnName;                
                switch ( Tabla.Columns[iCol].DataType.Name ) { 
                    case "DateTime":
                        sColumnFormat = "dd/MM/aaaa";
                    break;
                    case "TimeSpan":
                        sColumnFormat = "hh:mm:ss";
                    break;
                    case "Decimal":
                        sColumnFormat = "$#,##0.00";
                    break;
                    
                    case "Int16":
                    case "Int32":
                    //case "Int64":
                        sColumnFormat = "#,##0";
                    break;
                }
                ((Microsoft.Office.Interop.Excel.Range)xlsHoja.Cells[1, iCol + 1]).EntireColumn.NumberFormat = sColumnFormat;
            }

            Microsoft.Office.Interop.Excel.Range tableRange = xlsHoja.get_Range("A2", "A2");
            
            tableRange = tableRange.get_Resize(Tabla.Rows.Count, Tabla.Columns.Count);

            object[,] moTabla = new object[Tabla.Rows.Count, Tabla.Columns.Count];

            for ( int iCol = 0; iCol < moTabla.GetLength(1); iCol++ )
                if ( Tabla.Columns[iCol].DataType.Name == "TimeSpan")
                    for ( int iFila = 0; iFila < moTabla.GetLength(0); iFila++ )                
                        moTabla[iFila, iCol] = Tabla.Rows[iFila][iCol].ToString();
                else
                    for ( int iFila = 0; iFila < moTabla.GetLength(0); iFila++ )
                        moTabla[iFila, iCol] = Tabla.Rows[iFila][iCol];
   
            try {
                tableRange.Value2 = moTabla;
                tableRange.EntireColumn.AutoFit();
            } catch ( Exception ex ) {
                sResultado = "Falló la exportación de los datos a excel.\r\n" + ex.Message;
            }

            try {
                if (sResultado =="")
                    xlsLibro.SaveCopyAs(filePath);
            } catch ( Exception ex ) {
                sResultado = "Falló el guardado del archivo, verifique no esté en uso.\r\n" + ex.Message;
            }

            xlsVentana.Visible = true;
            
            //xlsLibro.Close(false, "", 0);
            //xlsVentana.Quit();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsVentana);
            //xlsVentana = null;
             

            CleanMemory();

            return sResultado;
        }
        
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize")]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        private static void CleanMemory() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            int iMinMB = 15 * ( 1024 ^ 3 );
            int iMaxMB = 230 * ( 1024 ^ 3 );

            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, iMinMB, iMaxMB);
        }
        
    }*/
    #endregion

}

