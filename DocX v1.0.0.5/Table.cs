﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Novacode;
using System.IO.Packaging;
using System.IO;
using System.Reflection;

namespace Novacode
{
    /// <summary>
    /// Designs\Styles that can be applied to a table.
    /// </summary>
    public enum TableDesign { TableNormal, TableGrid, LightShading, LightShadingAccent1, LightShadingAccent2, LightShadingAccent3, LightShadingAccent4, LightShadingAccent5, LightShadingAccent6, LightList, LightListAccent1, LightListAccent2, LightListAccent3, LightListAccent4, LightListAccent5, LightListAccent6, LightGrid, LightGridAccent1, LightGridAccent2, LightGridAccent3, LightGridAccent4, LightGridAccent5, LightGridAccent6, MediumShading1, MediumShading1Accent1, MediumShading1Accent2, MediumShading1Accent3, MediumShading1Accent4, MediumShading1Accent5, MediumShading1Accent6, MediumShading2, MediumShading2Accent1, MediumShading2Accent2, MediumShading2Accent3, MediumShading2Accent4, MediumShading2Accent5, MediumShading2Accent6, MediumList1, MediumList1Accent1, MediumList1Accent2, MediumList1Accent3, MediumList1Accent4, MediumList1Accent5, MediumList1Accent6, MediumList2, MediumList2Accent1, MediumList2Accent2, MediumList2Accent3, MediumList2Accent4, MediumList2Accent5, MediumList2Accent6, MediumGrid1, MediumGrid1Accent1, MediumGrid1Accent2, MediumGrid1Accent3, MediumGrid1Accent4, MediumGrid1Accent5, MediumGrid1Accent6, MediumGrid2, MediumGrid2Accent1, MediumGrid2Accent2, MediumGrid2Accent3, MediumGrid2Accent4, MediumGrid2Accent5, MediumGrid2Accent6, MediumGrid3, MediumGrid3Accent1, MediumGrid3Accent2, MediumGrid3Accent3, MediumGrid3Accent4, MediumGrid3Accent5, MediumGrid3Accent6, DarkList, DarkListAccent1, DarkListAccent2, DarkListAccent3, DarkListAccent4, DarkListAccent5, DarkListAccent6, ColorfulShading, ColorfulShadingAccent1, ColorfulShadingAccent2, ColorfulShadingAccent3, ColorfulShadingAccent4, ColorfulShadingAccent5, ColorfulShadingAccent6, ColorfulList, ColorfulListAccent1, ColorfulListAccent2, ColorfulListAccent3, ColorfulListAccent4, ColorfulListAccent5, ColorfulListAccent6, ColorfulGrid, ColorfulGridAccent1, ColorfulGridAccent2, ColorfulGridAccent3, ColorfulGridAccent4, ColorfulGridAccent5, ColorfulGridAccent6};

    /// <summary>
    /// Represents a Table in a document.
    /// </summary>
    public class Table
    {
        private List<Row> rows;
        private int rowCount, columnCount;
        internal XElement xml;

        /// <summary>
        /// Returns the number of rows in this table.
        /// </summary>
        public int RowCount { get { return rowCount; } }

        /// <summary>
        /// Returns the number of coloumns in this table.
        /// </summary>
        public int ColumnCount { get { return columnCount; } }

        /// <summary>
        /// Returns a list of rows in this table.
        /// </summary>
        public List<Row> Rows { get { return rows; } }
        DocX document;
        private TableDesign design;

        internal Table(DocX document, XElement xml)
        {
            this.xml = xml;
            this.document = document;

            XElement properties = xml.Element(XName.Get("tblPr", DocX.w.NamespaceName));
            
            rows = (from r in xml.Elements(XName.Get("tr", DocX.w.NamespaceName))
                       select new Row(document, r)).ToList();

            rowCount = rows.Count;

            if (rows.Count > 0)
                if (rows[0].Cells.Count > 0)
                    columnCount = rows[0].Cells.Count;

            XElement style = properties.Element(XName.Get("tblStyle", DocX.w.NamespaceName));
            XAttribute val = style.Attribute(XName.Get("val", DocX.w.NamespaceName));
            design = (TableDesign)Enum.Parse(typeof(TableDesign), val.Value.Replace("-", string.Empty));
        }

        /// <summary>
        /// The design\style to apply to this table.
        /// </summary>
        public TableDesign Design 
        {
            get { return design; }
            set
            {
                XElement tblPr = xml.Element(XName.Get("tblPr", DocX.w.NamespaceName));
                XElement style = tblPr.Element(XName.Get("tblStyle", DocX.w.NamespaceName));
                if (style == null)
                {
                    tblPr.Add(new XElement(XName.Get("tblStyle", DocX.w.NamespaceName)));
                    style = tblPr.Element(XName.Get("tblStyle", DocX.w.NamespaceName));
                }

                XAttribute val = style.Attribute(XName.Get("val", DocX.w.NamespaceName));
                if(val == null)
                {
                    style.Add(new XAttribute(XName.Get("val", DocX.w.NamespaceName), ""));
                    val = style.Attribute(XName.Get("val", DocX.w.NamespaceName));
                }

                design = value;

                switch (design)
                {
                    case TableDesign.TableNormal: val.Value = "TableNormal"; break;
                    case TableDesign.TableGrid: val.Value = "TableGrid"; break;
                    case TableDesign.LightShading: val.Value = "LightShading"; break;
                    case TableDesign.LightShadingAccent1: val.Value = "LightShading-Accent1"; break;
                    case TableDesign.LightShadingAccent2: val.Value = "LightShading-Accent2"; break;
                    case TableDesign.LightShadingAccent3: val.Value = "LightShading-Accent3"; break;
                    case TableDesign.LightShadingAccent4: val.Value = "LightShading-Accent4"; break;
                    case TableDesign.LightShadingAccent5: val.Value = "LightShading-Accent5"; break;
                    case TableDesign.LightShadingAccent6: val.Value = "LightShading-Accent6"; break;
                    case TableDesign.LightList: val.Value = "LightList"; break;
                    case TableDesign.LightListAccent1: val.Value = "LightList-Accent1"; break;
                    case TableDesign.LightListAccent2: val.Value = "LightList-Accent2"; break;
                    case TableDesign.LightListAccent3: val.Value = "LightList-Accent3"; break;
                    case TableDesign.LightListAccent4: val.Value = "LightList-Accent4"; break;
                    case TableDesign.LightListAccent5: val.Value = "LightList-Accent5"; break;
                    case TableDesign.LightListAccent6: val.Value = "LightList-Accent6"; break;
                    case TableDesign.LightGrid: val.Value = "LightGrid"; break;
                    case TableDesign.LightGridAccent1: val.Value = "LightGrid-Accent1"; break;
                    case TableDesign.LightGridAccent2: val.Value = "LightGrid-Accent2"; break;
                    case TableDesign.LightGridAccent3: val.Value = "LightGrid-Accent3"; break;
                    case TableDesign.LightGridAccent4: val.Value = "LightGrid-Accent4"; break;
                    case TableDesign.LightGridAccent5: val.Value = "LightGrid-Accent5"; break;
                    case TableDesign.LightGridAccent6: val.Value = "LightGrid-Accent6"; break;
                    case TableDesign.MediumShading1: val.Value = "MediumShading1"; break;
                    case TableDesign.MediumShading1Accent1: val.Value = "MediumShading1-Accent1"; break;
                    case TableDesign.MediumShading1Accent2: val.Value = "MediumShading1-Accent2"; break;
                    case TableDesign.MediumShading1Accent3: val.Value = "MediumShading1-Accent3"; break;
                    case TableDesign.MediumShading1Accent4: val.Value = "MediumShading1-Accent4"; break;
                    case TableDesign.MediumShading1Accent5: val.Value = "MediumShading1-Accent5"; break;
                    case TableDesign.MediumShading1Accent6: val.Value = "MediumShading1-Accent6"; break;
                    case TableDesign.MediumShading2: val.Value = "MediumShading2"; break;
                    case TableDesign.MediumShading2Accent1: val.Value = "MediumShading2-Accent1"; break;
                    case TableDesign.MediumShading2Accent2: val.Value = "MediumShading2-Accent2"; break;
                    case TableDesign.MediumShading2Accent3: val.Value = "MediumShading2-Accent3"; break;
                    case TableDesign.MediumShading2Accent4: val.Value = "MediumShading2-Accent4"; break;
                    case TableDesign.MediumShading2Accent5: val.Value = "MediumShading2-Accent5"; break;
                    case TableDesign.MediumShading2Accent6: val.Value = "MediumShading2-Accent6"; break;
                    case TableDesign.MediumList1: val.Value = "MediumList1"; break;
                    case TableDesign.MediumList1Accent1: val.Value = "MediumList1-Accent1"; break;
                    case TableDesign.MediumList1Accent2: val.Value = "MediumList1-Accent2"; break;
                    case TableDesign.MediumList1Accent3: val.Value = "MediumList1-Accent3"; break;
                    case TableDesign.MediumList1Accent4: val.Value = "MediumList1-Accent4"; break;
                    case TableDesign.MediumList1Accent5: val.Value = "MediumList1-Accent5"; break;
                    case TableDesign.MediumList1Accent6: val.Value = "MediumList1-Accent6"; break;
                    case TableDesign.MediumList2: val.Value = "MediumList2"; break;
                    case TableDesign.MediumList2Accent1: val.Value = "MediumList2-Accent1"; break;
                    case TableDesign.MediumList2Accent2: val.Value = "MediumList2-Accent2"; break;
                    case TableDesign.MediumList2Accent3: val.Value = "MediumList2-Accent3"; break;
                    case TableDesign.MediumList2Accent4: val.Value = "MediumList2-Accent4"; break;
                    case TableDesign.MediumList2Accent5: val.Value = "MediumList2-Accent5"; break;
                    case TableDesign.MediumList2Accent6: val.Value = "MediumList2-Accent6"; break;
                    case TableDesign.MediumGrid1: val.Value = "MediumGrid1"; break;
                    case TableDesign.MediumGrid1Accent1: val.Value = "MediumGrid1-Accent1"; break;
                    case TableDesign.MediumGrid1Accent2: val.Value = "MediumGrid1-Accent2"; break;
                    case TableDesign.MediumGrid1Accent3: val.Value = "MediumGrid1-Accent3"; break;
                    case TableDesign.MediumGrid1Accent4: val.Value = "MediumGrid1-Accent4"; break;
                    case TableDesign.MediumGrid1Accent5: val.Value = "MediumGrid1-Accent5"; break;
                    case TableDesign.MediumGrid1Accent6: val.Value = "MediumGrid1-Accent6"; break;
                    case TableDesign.MediumGrid2: val.Value = "MediumGrid2"; break;
                    case TableDesign.MediumGrid2Accent1: val.Value = "MediumGrid2-Accent1"; break;
                    case TableDesign.MediumGrid2Accent2: val.Value = "MediumGrid2-Accent2"; break;
                    case TableDesign.MediumGrid2Accent3: val.Value = "MediumGrid2-Accent3"; break;
                    case TableDesign.MediumGrid2Accent4: val.Value = "MediumGrid2-Accent4"; break;
                    case TableDesign.MediumGrid2Accent5: val.Value = "MediumGrid2-Accent5"; break;
                    case TableDesign.MediumGrid2Accent6: val.Value = "MediumGrid2-Accent6"; break;
                    case TableDesign.MediumGrid3: val.Value = "MediumGrid3"; break;
                    case TableDesign.MediumGrid3Accent1: val.Value = "MediumGrid3-Accent1"; break;
                    case TableDesign.MediumGrid3Accent2: val.Value = "MediumGrid3-Accent2"; break;
                    case TableDesign.MediumGrid3Accent3: val.Value = "MediumGrid3-Accent3"; break;
                    case TableDesign.MediumGrid3Accent4: val.Value = "MediumGrid3-Accent4"; break;
                    case TableDesign.MediumGrid3Accent5: val.Value = "MediumGrid3-Accent5"; break;
                    case TableDesign.MediumGrid3Accent6: val.Value = "MediumGrid3-Accent6"; break;

                    case TableDesign.DarkList: val.Value = "DarkList"; break;
                    case TableDesign.DarkListAccent1: val.Value = "DarkList-Accent1"; break;
                    case TableDesign.DarkListAccent2: val.Value = "DarkList-Accent2"; break;
                    case TableDesign.DarkListAccent3: val.Value = "DarkList-Accent3"; break;
                    case TableDesign.DarkListAccent4: val.Value = "DarkList-Accent4"; break;
                    case TableDesign.DarkListAccent5: val.Value = "DarkList-Accent5"; break;
                    case TableDesign.DarkListAccent6: val.Value = "DarkList-Accent6"; break;

                    case TableDesign.ColorfulShading: val.Value = "ColorfulShading"; break;
                    case TableDesign.ColorfulShadingAccent1: val.Value = "ColorfulShading-Accent1"; break;
                    case TableDesign.ColorfulShadingAccent2: val.Value = "ColorfulShading-Accent2"; break;
                    case TableDesign.ColorfulShadingAccent3: val.Value = "ColorfulShading-Accent3"; break;
                    case TableDesign.ColorfulShadingAccent4: val.Value = "ColorfulShading-Accent4"; break;
                    case TableDesign.ColorfulShadingAccent5: val.Value = "ColorfulShading-Accent5"; break;
                    case TableDesign.ColorfulShadingAccent6: val.Value = "ColorfulShading-Accent6"; break;

                    case TableDesign.ColorfulList: val.Value = "ColorfulList"; break;
                    case TableDesign.ColorfulListAccent1: val.Value = "ColorfulList-Accent1"; break;
                    case TableDesign.ColorfulListAccent2: val.Value = "ColorfulList-Accent2"; break;
                    case TableDesign.ColorfulListAccent3: val.Value = "ColorfulList-Accent3"; break;
                    case TableDesign.ColorfulListAccent4: val.Value = "ColorfulList-Accent4"; break;
                    case TableDesign.ColorfulListAccent5: val.Value = "ColorfulList-Accent5"; break;
                    case TableDesign.ColorfulListAccent6: val.Value = "ColorfulList-Accent6"; break;

                    case TableDesign.ColorfulGrid: val.Value = "ColorfulGrid"; break;
                    case TableDesign.ColorfulGridAccent1: val.Value = "ColorfulGrid-Accent1"; break;
                    case TableDesign.ColorfulGridAccent2: val.Value = "ColorfulGrid-Accent2"; break;
                    case TableDesign.ColorfulGridAccent3: val.Value = "ColorfulGrid-Accent3"; break;
                    case TableDesign.ColorfulGridAccent4: val.Value = "ColorfulGrid-Accent4"; break;
                    case TableDesign.ColorfulGridAccent5: val.Value = "ColorfulGrid-Accent5"; break;
                    case TableDesign.ColorfulGridAccent6: val.Value = "ColorfulGrid-Accent6"; break;

                    default: break;
                }

                XDocument style_doc;
                PackagePart word_styles = document.package.GetPart(new Uri("/word/styles.xml", UriKind.Relative));
                using (TextReader tr = new StreamReader(word_styles.GetStream()))
                    style_doc = XDocument.Load(tr);

                var tableStyle =
                (
                    from e in style_doc.Descendants()
                    let styleId = e.Attribute(XName.Get("styleId", DocX.w.NamespaceName))
                    where (styleId != null && styleId.Value == val.Value)
                    select e
                ).FirstOrDefault();

                if (tableStyle == null)
                {
                    Assembly _assembly = Assembly.GetExecutingAssembly();
                    TextReader _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("Novacode.styles.xml"));
                    XDocument external_style_doc = XDocument.Load(_textStreamReader);

                    var styleElement =
                    (
                        from e in external_style_doc.Descendants()
                        let styleId = e.Attribute(XName.Get("styleId", DocX.w.NamespaceName))
                        where (styleId != null && styleId.Value == val.Value)
                        select e
                    ).First();

                    style_doc.Element(XName.Get("styles", DocX.w.NamespaceName)).Add(styleElement);

                    using (TextWriter tw = new StreamWriter(word_styles.GetStream(FileMode.Create)))
                        style_doc.Save(tw, SaveOptions.None);
                }
            }
        }

        /// <summary>
        /// Insert a row at the end of this table.
        /// </summary>
        /// <example>
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///        
        ///     // Insert a new row at the end of this table.
        ///     Row row = table.InsertRow();
        ///
        ///     // Loop through each cell in this new row.
        ///     foreach (Cell c in row.Cells)
        ///     {
        ///         // Set the text of each new cell to "Hello".
        ///         c.Paragraph.InsertText("Hello", false);
        ///     }
        ///
        ///     // Save the document to a new file.
        ///     document.SaveAs(@"C:\Example\Test2.docx");
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        /// <returns>A new row.</returns>
        public Row InsertRow()
        {
            return InsertRow(rows.Count);
        }

        /// <summary>
        /// Insert a column to the right of a Table.
        /// </summary>
        /// <example>
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first Table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Insert a new column to this right of this table.
        ///     table.InsertColumn();
        ///
        ///     // Set the new coloumns text to "Row no."
        ///     table.Rows[0].Cells[table.ColumnCount - 1].Paragraph.InsertText("Row no.", false);
        ///
        ///     // Loop through each row in the table.
        ///     for (int i = 1; i &lt; table.Rows.Count; i++)
        ///     {
        ///         // The current row.
        ///         Row row = table.Rows[i];
        ///
        ///         // The cell in this row that belongs to the new coloumn.
        ///         Cell cell = row.Cells[table.ColumnCount - 1];
        ///
        ///         // The Paragraph that this cell houses.
        ///         Paragraph p = cell.Paragraph;
        ///
        ///         // Insert this rows index.
        ///         p.InsertText(i.ToString(), false);
        ///     }
        ///
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void InsertColumn()
        {
            InsertColumn(columnCount);
        }

        /// <summary>
        /// Remove the last row from this Table.
        /// </summary>
        /// <example>
        /// Remove the last row from a Table.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Remove the last row from this table.
        ///     table.RemoveRow();
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void RemoveRow()
        {
            RemoveRow(rowCount - 1);
        }

        /// <summary>
        /// Remove a row from this Table.
        /// </summary>
        /// <param name="index">The row to remove.</param>
        /// <example>
        /// Remove the first row from a Table.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Remove the first row from this table.
        ///     table.RemoveRow(0);
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void RemoveRow(int index)
        {
            if (index < 0 || index > rows.Count)
                throw new IndexOutOfRangeException();

            rows[index].xml.Remove();
        }

        /// <summary>
        /// Remove the last column for this Table.
        /// </summary>
        /// <example>
        /// Remove the last column from a Table.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Remove the last column from this table.
        ///     table.RemoveColumn();
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void RemoveColumn()
        {
            RemoveColumn(columnCount - 1);
        }

        /// <summary>
        /// Remove a coloumn from this Table.
        /// </summary>
        /// <param name="index">The coloumn to remove.</param>
        /// <example>
        /// Remove the first column from a Table.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Remove the first column from this table.
        ///     table.RemoveColumn(0);
        ///
        ///     // Save the document.
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void RemoveColumn(int index)
        {
            if (index < 0 || index > columnCount - 1)
                throw new IndexOutOfRangeException();

            foreach (Row r in rows)
                r.Cells[index].xml.Remove();
        }

        /// <summary>
        /// Insert a row into this table.
        /// </summary>
        /// <example>
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first table in this document.
        ///     Table table = document.Tables[0];
        ///        
        ///     // Insert a new row at index 1 in this table.
        ///     Row row = table.InsertRow(1);
        ///
        ///     // Loop through each cell in this new row.
        ///     foreach (Cell c in row.Cells)
        ///     {
        ///         // Set the text of each new cell to "Hello".
        ///         c.Paragraph.InsertText("Hello", false);
        ///     }
        ///
        ///     // Save the document to a new file.
        ///     document.SaveAs(@"C:\Example\Test2.docx");
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        /// <param name="index">Index to insert row at.</param>
        /// <returns>A new Row</returns>
        public Row InsertRow(int index)
        {
            if (index < 0 || index > rows.Count)
                throw new IndexOutOfRangeException();

            List<XElement> content = new List<XElement>();
                        
            foreach (Cell c in rows[0].Cells)
                content.Add(new XElement(XName.Get("tc", DocX.w.NamespaceName), new XElement(XName.Get("p", DocX.w.NamespaceName))));

            XElement e = new XElement(XName.Get("tr", DocX.w.NamespaceName), content);
            Row newRow = new Row(document, e);

            XElement rowXml;
            if (index == rows.Count)
            {
                rowXml = rows.Last().xml;
                rowXml.AddAfterSelf(newRow.xml);
            }
            
            else
            {
                rowXml = rows[index].xml;
                rowXml.AddBeforeSelf(newRow.xml);
            }

            rows.Insert(index, newRow);
            rowCount = rows.Count;
            return newRow;
        }

        /// <summary>
        /// Insert a column into a table.
        /// </summary>
        /// <param name="index">The index to insert the column at.</param>
        /// <example>
        /// Insert a column to the left of a table.
        /// <code>
        /// // Load a document.
        /// using (DocX document = DocX.Load(@"C:\Example\Test.docx"))
        /// {
        ///     // Get the first Table in this document.
        ///     Table table = document.Tables[0];
        ///
        ///     // Insert a new column to this left of this table.
        ///     table.InsertColumn(0);
        ///
        ///     // Set the new coloumns text to "Row no."
        ///     table.Rows[0].Cells[table.ColumnCount - 1].Paragraph.InsertText("Row no.", false);
        ///
        ///     // Loop through each row in the table.
        ///     for (int i = 1; i &lt; table.Rows.Count; i++)
        ///     {
        ///         // The current row.
        ///         Row row = table.Rows[i];
        ///
        ///         // The cell in this row that belongs to the new coloumn.
        ///         Cell cell = row.Cells[table.ColumnCount - 1];
        ///
        ///         // The Paragraph that this cell houses.
        ///         Paragraph p = cell.Paragraph;
        ///
        ///         // Insert this rows index.
        ///         p.InsertText(i.ToString(), false);
        ///     }
        ///
        ///     document.Save();
        /// }// Release this document from memory.
        /// </code>
        /// </example>
        public void InsertColumn(int index)
        {
            if (rows.Count > 0)
            {
                foreach (Row r in rows)
                {
                    if(columnCount == index)
                        r.Cells[index - 1].xml.AddAfterSelf(new XElement(XName.Get("tc", DocX.w.NamespaceName), new XElement(XName.Get("p", DocX.w.NamespaceName))));
                    else
                        r.Cells[index].xml.AddBeforeSelf(new XElement(XName.Get("tc", DocX.w.NamespaceName), new XElement(XName.Get("p", DocX.w.NamespaceName))));
                }

                rows = (from r in xml.Elements(XName.Get("tr", DocX.w.NamespaceName))
                        select new Row(document, r)).ToList();

                rowCount = rows.Count;

                if (rows.Count > 0)
                    if (rows[0].Cells.Count > 0)
                        columnCount = rows[0].Cells.Count;
            }
        }
    }

    /// <summary>
    /// Represents a single row in a Table.
    /// </summary>
    public class Row
    {
        DocX document;
        internal XElement xml;
        private List<Cell> cells;

        /// <summary>
        /// A list of Cells in this Row.
        /// </summary>
        public List<Cell> Cells { get { return cells; } }

        internal Row(DocX document, XElement xml)
        {
            this.document = document;
            this.xml = xml;
            cells = (from c in xml.Elements(XName.Get("tc", DocX.w.NamespaceName))
                     select new Cell(document, c)).ToList();
        }
    }

    public class Cell
    {
        private Paragraph p;
        private DocX document;
        internal XElement xml;

        public Paragraph Paragraph
        {
            get { return p; }
            set { p = value; }
        }

        internal Cell(DocX document, XElement xml)
        {
            this.document = document;
            this.xml = xml;

            XElement properties = xml.Element(XName.Get("tcPr", DocX.w.NamespaceName));

            p = new Paragraph(document, 0, xml.Element(XName.Get("p", DocX.w.NamespaceName)));
        }
    }
}