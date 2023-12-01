using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace CadExtension
{
    public class Commands
    {
        private Database db; // Database nesnesini tanımlıyoruz

        [CommandMethod("DrawColumn")]
        public void DrawColumn()
        {
            // AutoCAD uygulamasını başlat
            Document doc = Application.DocumentManager.MdiActiveDocument;
            db = doc.Database; // Database nesnesini bu metod içinde tanımlıyoruz
            Editor ed = doc.Editor;

            // Dikdörtgen, daire ve metin eklemek için başlangıç ve bitiş noktalarını belirlemek için kullandığımız değişkenler
            double KesitGen = 50;
            double KesitYuk = 50;
            double KolonYuk = 150;
            double Paspayı = 2.5;
            double BoyunaDonatıCapı = 0.5;
            double EtriyeAralık = 15;

            Point3d startPoint1r = new Point3d(0, 0, 0);
            Point3d endPoint1r = new Point3d(KesitGen, KesitYuk, 0);
            Point3d startPoint2r = new Point3d(Paspayı, Paspayı, 0);
            Point3d endPoint2r = new Point3d(KesitGen - Paspayı, KesitYuk - Paspayı, 0);
            Point3d startPoint3r = new Point3d(KesitGen + 50, 0, 0);
            Point3d endPoint3r = new Point3d(2 * KesitGen + 50, KolonYuk, 0);
            Point3d startPoint4r = new Point3d(KesitGen + 50 + Paspayı, 0, 0);
            Point3d endPoint4r = new Point3d(2 * KesitGen + 50 - Paspayı, KolonYuk, 0);


            Point3d startPointc1 = new Point3d(Paspayı + BoyunaDonatıCapı, Paspayı + BoyunaDonatıCapı, 0);
            Point3d startPointc2 = new Point3d(KesitGen / 2, Paspayı + BoyunaDonatıCapı, 0);
            Point3d startPointc3 = new Point3d(KesitGen - (Paspayı + BoyunaDonatıCapı), Paspayı + BoyunaDonatıCapı, 0);
            Point3d startPointc4 = new Point3d(Paspayı + BoyunaDonatıCapı, KesitYuk - (Paspayı + BoyunaDonatıCapı), 0);
            Point3d startPointc5 = new Point3d(KesitGen / 2, KesitYuk - (Paspayı + BoyunaDonatıCapı), 0);
            Point3d startPointc6 = new Point3d(KesitGen - (Paspayı + BoyunaDonatıCapı), KesitYuk - (Paspayı + BoyunaDonatıCapı), 0);

            Point3d endPointt1 = new Point3d(00, -50, 0);
            Point3d endPointt2 = new Point3d(00, -65, 0);
            Point3d endPointt3 = new Point3d(00, -80, 0);
            Point3d endPointt4 = new Point3d(00, KesitYuk + 20, 0);
            Point3d endPointt5 = new Point3d(-80, KesitYuk / 2, 0);
            Point3d endPointt6 = new Point3d(KesitGen + 50, KolonYuk + 20, 0);
            Point3d endPointt7 = new Point3d(2 * KesitGen + 70, KolonYuk / 2, 0);


            // Dikdörtgen çiz
            DrawRectangle(startPoint1r, endPoint1r);
            DrawRectangle(startPoint2r, endPoint2r);
            DrawRectangle(startPoint3r, endPoint3r);
            DrawRectangle(startPoint4r, endPoint4r);

            //Çizgi çiz
            double EtriyeSay = Math.Ceiling(KolonYuk / EtriyeAralık);
            Point3d[] lineStartPoints = new Point3d[(int)EtriyeSay];
            Point3d[] lineEndPoints = new Point3d[(int)EtriyeSay];

            for (int i = 0; i < EtriyeSay; i++)
            {
                lineStartPoints[i] = new Point3d(KesitGen + 50 + Paspayı, i * EtriyeAralık, 0);
                lineEndPoints[i] = new Point3d(2 * KesitGen + 50 - Paspayı, i * EtriyeAralık, 0);
            }

            DrawLine(lineStartPoints, lineEndPoints);

            // Daire çiz
            DrawCircle(startPointc1, BoyunaDonatıCapı);
            DrawCircle(startPointc2, BoyunaDonatıCapı);
            DrawCircle(startPointc3, BoyunaDonatıCapı);
            DrawCircle(startPointc4, BoyunaDonatıCapı);
            DrawCircle(startPointc5, BoyunaDonatıCapı);
            DrawCircle(startPointc6, BoyunaDonatıCapı);


            // Metin ekle
            DrawText(endPointt1, "Pas Payı Bilgisi(2,5cm)", 10);
            DrawText(endPointt2, "Boyuna Donatı Bilgisi(12f16)", 10);
            DrawText(endPointt3, "Etriye Donatı Bilgisi(Çap-sıkılaştırma aralığı(f16/15/8)", 10);
            DrawText(endPointt4, "kolon kesit genisliği x", 10);
            DrawText(endPointt5, "kolon kesit yüksekliği y", 10);
            DrawText(endPointt6, "kolon genişliği", 10);
            DrawText(endPointt7, "kolon yüksekliği", 10);
        }

        private void DrawRectangle(Point3d startPoint1r, Point3d endPoint1r)
        {
            // Dikdörtgeni çiz
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                Polyline rectangle = new Polyline(4);
                rectangle.AddVertexAt(0, new Point2d(startPoint1r.X, startPoint1r.Y), 0, 0, 0);
                rectangle.AddVertexAt(1, new Point2d(endPoint1r.X, startPoint1r.Y), 0, 0, 0);
                rectangle.AddVertexAt(2, new Point2d(endPoint1r.X, endPoint1r.Y), 0, 0, 0);
                rectangle.AddVertexAt(3, new Point2d(startPoint1r.X, endPoint1r.Y), 0, 0, 0);
                rectangle.Closed = true;

                btr.AppendEntity(rectangle);
                tr.AddNewlyCreatedDBObject(rectangle, true);

                tr.Commit();
            }
        }

        private void DrawLine(Point3d[] startPoints, Point3d[] endPoints)
        {
            // Çizgileri çiz
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                for (int i = 0; i < startPoints.Length; i++)
                {
                    Line line = new Line(startPoints[i], endPoints[i]);
                    btr.AppendEntity(line);
                    tr.AddNewlyCreatedDBObject(line, true);
                }

                tr.Commit();
            }
        }

        private void DrawCircle(Point3d center, double radius)
        {
            // Daireyi çiz
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                Circle circle = new Circle(center, Vector3d.ZAxis, radius);

                btr.AppendEntity(circle);
                tr.AddNewlyCreatedDBObject(circle, true);

                tr.Commit();
            }
        }



        private void DrawText(Point3d position, string text, double textSize)
        {
            // Metni ekle
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                DBText dbText = new DBText();
                dbText.Position = position;
                dbText.TextString = text;

                // Metin boyutunu belirle
                textSize = 5;
                dbText.Height = textSize;

                btr.AppendEntity(dbText);
                tr.AddNewlyCreatedDBObject(dbText, true);

                tr.Commit();
            }
        }
    }
}
