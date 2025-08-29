using System;
using System.Collections.Generic;
using System.IO;
using Biblioteca.Core.Models;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Biblioteca.Services
{
    public class WordDocumentService
    {
        public byte[] GenerateReminderLetter(List<Libro> libri, List<Autore> autori, List<Nazione> paesi, List<Lingua> lingue, Utente utente)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Crea un nuovo documento Word
                using (var document = DocX.Create(memoryStream))
                {
                    // Titolo principale
                    var titleParagraph = document.InsertParagraph("Avviso di Restituzione Libri")
                        .FontSize(20d)
                        .Bold();
                    titleParagraph.Alignment = Alignment.center;
                    titleParagraph.SpacingAfter(30d);

                    // Data generazione
                    var dateParagraph = document.InsertParagraph($"Data: {DateTime.Now:dd/MM/yyyy}")
                        .FontSize(12d)
                        .Italic();
                    dateParagraph.Alignment = Alignment.right;
                    dateParagraph.SpacingAfter(20d);

                    // Corpo della lettera
                    var bodyParagraph = document.InsertParagraph();
                    bodyParagraph.AppendLine($"Gentile {utente.Nome} {utente.Cognome},")
                                 .SpacingAfter(10d);
                    bodyParagraph.AppendLine("si avvisa l'utente che in data odierna i seguenti libri risultano in scadenza per la restituzione:")
                                 .SpacingAfter(15d);

                    // Tabella dei libri
                    if (libri != null && libri.Count > 0)
                    {
                        var libriTable = document.AddTable(libri.Count + 1, 7);
                        libriTable.Design = TableDesign.ColorfulListAccent1;
                        libriTable.Alignment = Alignment.left;

                        // Intestazioni tabella
                        libriTable.Rows[0].Cells[0].Paragraphs[0].Append("Titolo").Bold();
                        libriTable.Rows[0].Cells[1].Paragraphs[0].Append("Autore").Bold();
                        libriTable.Rows[0].Cells[2].Paragraphs[0].Append("Anno").Bold();
                        libriTable.Rows[0].Cells[3].Paragraphs[0].Append("Paese").Bold();
                        libriTable.Rows[0].Cells[4].Paragraphs[0].Append("Lingua").Bold();
                        libriTable.Rows[0].Cells[5].Paragraphs[0].Append("Prezzo").Bold();
                        libriTable.Rows[0].Cells[6].Paragraphs[0].Append("Pagine").Bold();

                        // Popola tabella
                        for (int i = 0; i < libri.Count; i++)
                        {
                            var libro = libri[i];
                            var autore = autori[i];
                            var paese = paesi[i];
                            var lingua = lingue[i];
                            libriTable.Rows[i + 1].Cells[0].Paragraphs[0].Append(libro.Titolo);
                            libriTable.Rows[i + 1].Cells[1].Paragraphs[0].Append(autore.Nome);
                            libriTable.Rows[i + 1].Cells[2].Paragraphs[0].Append(libro.Anno.ToString());
                            libriTable.Rows[i + 1].Cells[3].Paragraphs[0].Append(paese.Nome);
                            libriTable.Rows[i + 1].Cells[4].Paragraphs[0].Append(lingua.Nome);
                            libriTable.Rows[i + 1].Cells[5].Paragraphs[0].Append(libro.Prezzo.ToString("F2"));
                            libriTable.Rows[i + 1].Cells[6].Paragraphs[0].Append(libro.Pagine.ToString());
                        }

                        // Inserisci tabella nel documento
                        document.InsertTable(libriTable);
                        document.InsertParagraph().SpacingAfter(20d);
                    }

                    // Chiusura lettera
                    var closingParagraph = document.InsertParagraph();
                    closingParagraph.AppendLine("La invitiamo a restituire i libri al più presto presso la biblioteca.")
                                    .SpacingAfter(10d);
                    closingParagraph.AppendLine("Cordiali saluti,")
                                    .AppendLine("Staff Biblioteca");

                    // Salva il documento
                    document.SaveAs(memoryStream);
                }

                return memoryStream.ToArray();
            }
        }
    }
}
