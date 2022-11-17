using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Globalization;
using System.Web.UI.HtmlControls;
using CARGAR_EXCEL.Models;
using System.Collections;
using System.Web.Services;
using RestSharp;
using System.Net;
using System.Text.RegularExpressions;
using iTextSharp.text.html;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Diagnostics;

namespace CARGAR_EXCEL
{
    public partial class Default : System.Web.UI.Page
    {
        static storedProcedure sql = new storedProcedure("miConexion");
        public static FacLabControler facLabControler = new FacLabControler();
        public static string jsonFactura = "", idSucursal = "", idTipoFactura = "", IdApiEmpresa = "";
        public string leg;
        public static List<string> result = new List<string>();
        static string Fecha;
        static string Subtotal;
        static string Totalimptrasl;
        static string Totalimpreten;
        static string Descuentos;
        static string Total;
        static string FormaPago;
        static string Condipago;
        static string MetodoPago;
        static string Moneda;
        static string RFC;
        static string CodSAT;
        static string IdProducto;
        static string Producto;
        static string Origen;
        static string Destino;
        

        public static List<string> results = new List<string>();
        static HtmlTable table = new HtmlTable();

        static char[] caracter = { '|' };
        static string[] words;
        protected void Page_Load(object sender, EventArgs e)
        {
            string segmento = Request.QueryString["segmento"];
            var request7 = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis?folioEspecifico=" + segmento);
            var response7 = (HttpWebResponse)request7.GetResponse();
            var responseString7 = new StreamReader(response7.GetResponseStream()).ReadToEnd();

            List<ModelFact> separados7 = JsonConvert.DeserializeObject<List<ModelFact>>(responseString7);

            if (separados7 != null)
            {
                foreach (var elem in separados7)

                {
                    string Serie = elem.serie;
                    if (Serie != "TDRT" || Serie != "NCT" || Serie != "TDRZP")
                    {
                        string Folio = segmento;

                        string UUID = elem.uuid;
                        string Pdf_xml_descarga = "https://canal1.xsa.com.mx:9050" + elem.pdfAndXmlDownload;
                        string Pdf_descargaFactura = "https://canal1.xsa.com.mx:9050" + elem.pdfDownload;
                        string xlm_descargaFactura = "https://canal1.xsa.com.mx:9050" + elem.xmlDownload;
                        string cancelFactura = "";
                        string LegNum = segmento;
                        string Fecha = elem.fecha;
                        string Total = elem.monto;
                        string Moneda = elem.tipoMoneda;
                        string RFC = elem.rfc;
                        string Origen = "0";
                        string Destino = "";




                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "RCalert()", true);
                        string msg = "Carta Porte generada: " + segmento;
                        //Rcartaporte.Value = msg;
                        ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Carta Porte generada ', 'success')", true);
                        FolioBox.Text = Folio;
                        SerieBox.Text = Serie;
                        UUIDBox.Text = UUID;
                        ZipBox.NavigateUrl = Pdf_xml_descarga;
                        PdfBox.NavigateUrl = Pdf_descargaFactura;
                        xmlBox.NavigateUrl = xlm_descargaFactura;
                        
                    }

                }
            }

            
            
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx", true);

        }
        
    }
}