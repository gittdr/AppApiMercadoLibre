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

namespace CARGAR_EXCEL
{
    public partial class WebForm1 : System.Web.UI.Page
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
            card1.Visible = true;
            card2.Visible = false;
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            //string numero = NOrden.Text;
            //RCPorte(numero);
            GetToken();
            
        }
        public void GetToken()
        {
            //POST
            var client = new RestClient("https://api.mercadolibre.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");

            request.AddHeader("Content-Type", "application/json");
            var body = @"{
            " + "\n" +
                        @"       ""client_id"": 4017611389022000,
            " + "\n" +
                        @"       ""client_secret"": ""ZK5Iuxv7CSovbXwWKzoKQ3rjtZtbFq0o"",
            " + "\n" +
                        @"       ""grant_type"": ""client_credentials""
            " + "\n" +
                        @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = (RestResponse)client.Execute(request);

            dynamic resp = JObject.Parse(response.Content);
            string token = resp.access_token;
            string token_type = resp.token_type;
            string expires_in = resp.expires_in;
            string scope = resp.scope;
            string user_id = resp.user_id;
            GetDetails(token);
        }
        public void GetDetails(string token)
        {
            //2036231 / MXXQR1
            //2025473/middle-mile/facilities/MXXEM1
            //2036355/MXXEM1
            string Ai_orden = NOrden.Text;
            string segmento = Segmento.Text;
            string routeid = RouteID.Text;
            string facility = FacilityID.Text;
            string moreurl1 = "/middle-mile/facilities/";
            string moreurl2 = "/details";
            string urls1 = "https://api.mercadolibre.com/shipping/fiscal/MLM/routes/" + routeid + moreurl1 + facility + moreurl2;
            string uwe = "KGM";
            var client = new RestClient(urls1);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("cache-control", "no-cache");
            RestResponse response = (RestResponse)client.Execute(request);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<MLMCartaPorte>(response.Content);
            string id = data.id;
            string cost = data.cost;
            string fullname = data.recipient.full_name;
            string rfc = data.recipient.rfc;

            string fiscal_fullname = data.origin.fiscal_information.full_name;
            string fiscal_rfc = data.origin.fiscal_information.rfc;
            string fiscal_residences = data.origin.fiscal_information.fiscal_residence;

            string addressline = data.origin.address.address_line;
            string street_name = data.origin.address.street_name;
            string street_number = data.origin.address.street_number;
            string intersection = data.origin.address.intersection;
            string zip_code = data.origin.address.zip_code;
            string city_id = data.origin.address.city.id;
            string city_name = data.origin.address.city.name;
            string state_id = data.origin.address.state.id;
            string state_name = data.origin.address.state.name;
            string country_id = data.origin.address.country.id;
            string country_name = data.origin.address.country.name;
            string neig_id = data.origin.address.neighborhood.id;
            string neig_name = data.origin.address.neighborhood.name;
            string muni_id = data.origin.address.municipality.id;
            string muni_name = data.origin.address.municipality.name;

            //DESTINATION
            string dfiscal_fullname = data.destination.fiscal_information.full_name;
            string dfiscal_rfc = data.destination.fiscal_information.rfc;
            string dfiscal_residences = data.destination.fiscal_information.fiscal_residence;
            string daddressline = data.destination.address.address_line;
            string dstreet_name = data.destination.address.street_name;
            string dstreet_number = data.destination.address.street_number;
            string dintersection = data.destination.address.intersection;
            string dzip_code = data.destination.address.zip_code;
            string dcity_id = data.destination.address.city.id;
            string dcity_name = data.destination.address.city.name;
            string dstate_id = data.destination.address.state.id;
            string dstate_name = data.destination.address.state.name;
            string dcountry_id = data.destination.address.country.id;
            string dcountry_name = data.destination.address.country.name;
            string dneig_id = data.destination.address.neighborhood.id;
            string dneig_name = data.destination.address.neighborhood.name;
            string dmuni_id = data.destination.address.municipality.id;
            string dmuni_name = data.destination.address.municipality.name;
            //END DESTINATION


            //SHIPMENTS
            dynamic info = data.shipments;
            foreach (var item in info)
            {

                string ship_id = item.id;
                string ship_url = item.url;
                string moreurl = "/items/details";
                string urls = "https://api.mercadolibre.com/shipping/fiscal/MLM/shipments/" + ship_id + moreurl;
                var client2 = new RestClient(urls);

                var request2 = new RestRequest(Method.GET);
                request2.AddHeader("authorization", "Bearer " + token);
                request2.AddHeader("cache-control", "no-cache");
                RestResponse response2 = (RestResponse)client2.Execute(request2);
                string respuesta = response2.Content;
                var dataz = Newtonsoft.Json.JsonConvert.DeserializeObject<MLMCartaPorte>(respuesta);
                if (dataz.status == 0)
                {
                    DataTable otds = facLabControler.ExisteSegmentos(segmento);
                    if (otds.Rows.Count > 0)
                    {
                        foreach (DataRow isegm in otds.Rows)
                        {
                            dynamic elementos = dataz.package.items;
                            foreach (var ccitem in elementos)
                            {
                                string cate = ccitem.category;
                                if (cate == "1010101")
                                {
                                    cate = "01010101";
                                }
                                string descript = ccitem.description;
                                string unitcode = ccitem.unit_code;
                                string quanti = ccitem.quantity;
                                int heig = ccitem.dimensions.height;
                                int widht = ccitem.dimensions.width;
                                int length = ccitem.dimensions.length;
                                int weight = ccitem.dimensions.weight;
                                string weig = weight.ToString();

                                //Aqui va el sp para insertar las mercancias
                                InsertMerc(Ai_orden, id, cate, descript, weight, uwe, quanti, unitcode);
                                facLabControler.GetMerca(Ai_orden, segmento, cate, descript, weig, uwe, quanti, unitcode);
                            }
                            int segm = Int32.Parse(segmento);
                            var request28196 = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis?folioEspecifico=" + segm);
                            var response28196 = (HttpWebResponse)request28196.GetResponse();
                            var responseString28196 = new StreamReader(response28196.GetResponseStream()).ReadToEnd();

                            List<ModelFact> separados819 = JsonConvert.DeserializeObject<List<ModelFact>>(responseString28196);
                            //PASO 2 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                            if (separados819 != null)
                            {
                                foreach (var rlist in separados819)
                                {
                                    string serie = rlist.serie;
                                    if (serie == "TDRXP")
                                    {
                                        string tipomensaje = "9";
                                        DataTable updateLegs = facLabControler.UpdateLeg(segmento, tipomensaje);
                                        string titulo = "Error en el segmento: ";
                                        string mensaje = "Error la carta porte  ya fue timbrada";
                                        facLabControler.enviarNotificacion(segmento, titulo, mensaje);

                                    }
                                    else
                                    {
                                        DataTable res = facLabControler.GetSegmentoRepetido(segmento);
                                        //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                        if (res.Rows.Count > 0)
                                        {
                                            //string foliorepetido = item2["segmento"].ToString();
                                            //Console.WriteLine("El Folio ya esta timbrado" + foliorepetido);
                                            foreach (DataRow gsegt in res.Rows)
                                            {
                                                string resst = gsegt["Serie"].ToString();
                                                if (resst == "TDRXP")
                                                {
                                                    DataTable vstatus = facLabControler.ExisteStatus(segmento);
                                                    foreach (DataRow lstu in vstatus.Rows)
                                                    {
                                                        string estatus = lstu["estatus"].ToString();
                                                        int vsegm = Int32.Parse(estatus);

                                                        if (vsegm != 2)
                                                        {
                                                            string tipomensaje = "9";
                                                            DataTable updateLegs = facLabControler.UpdateLeg(segmento, tipomensaje);
                                                            string titulo = "Error en el segmento: ";
                                                            string mensaje = "Error la carta porte ya fue timbrada.";
                                                            facLabControler.enviarNotificacion(segmento, titulo, mensaje);
                                                        }
                                                        else
                                                        {
                                                            string titulo = "Error en el segmento: ";
                                                            string mensaje = "Error la carta porte ya fue timbrada.";
                                                            facLabControler.enviarNotificacion(segmento, titulo, mensaje);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    DataTable results = facLabControler.TieneMercancias(segmento);
                                                    //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                                    foreach (DataRow resl in results.Rows)
                                                    {
                                                        string totald = resl["total"].ToString();
                                                        int num_var = Int32.Parse(totald);
                                                        if (num_var > 0)
                                                        {

                                                            valida(segmento);

                                                        }
                                                    }
                                                }
                                            }


                                        }
                                        else  // PASO 5 - SI NO EXISTE CONTINUA CON EL PROCESO DE TIMBRADO
                                        {
                                            DataTable results = facLabControler.TieneMercancias(segmento);
                                            //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                            foreach (DataRow resl in results.Rows)
                                            {
                                                string totald = resl["total"].ToString();
                                                int num_var = Int32.Parse(totald);
                                                if (num_var > 0)
                                                {


                                                    valida(segmento);

                                                }
                                            }
                                        }

                                    }
                                }

                            }
                            else
                            {
                                //PASO 3 - VALIDA QUE NO ESTE REGISTRADO EN LA VISTA_CARTA_PORTE
                                DataTable res = facLabControler.GetSegmentoRepetido(segmento);
                                //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                if (res.Rows.Count > 0)
                                {
                                    //string foliorepetido = item2["segmento"].ToString();
                                    //Console.WriteLine("El Folio ya esta timbrado" + esegmento);
                                    foreach (DataRow gsegt in res.Rows)
                                    {
                                        string resst = gsegt["Serie"].ToString();
                                        if (resst == "TDRXP")
                                        {
                                            DataTable vstatus = facLabControler.ExisteStatus(segmento);
                                            foreach (DataRow lstu in vstatus.Rows)
                                            {
                                                string estatus = lstu["estatus"].ToString();
                                                int vsegm = Int32.Parse(estatus);

                                                if (vsegm != 2)
                                                {
                                                    string tipomensaje = "9";
                                                    DataTable updateLegs = facLabControler.UpdateLeg(segmento, tipomensaje);
                                                    string titulo = "Error en el segmento: ";
                                                    string mensaje = "Error la carta porte ya fue timbrada.";
                                                    facLabControler.enviarNotificacion(segmento, titulo, mensaje);
                                                }
                                                else
                                                {
                                                    string titulo = "Error en el segmento: ";
                                                    string mensaje = "Error la carta porte ya fue timbrada.";
                                                    facLabControler.enviarNotificacion(segmento, titulo, mensaje);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataTable results = facLabControler.TieneMercancias(segmento);
                                            //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                            foreach (DataRow resl in results.Rows)
                                            {
                                                string totald = resl["total"].ToString();
                                                int num_var = Int32.Parse(totald);
                                                if (num_var > 0)
                                                {

                                                    valida(segmento);

                                                }
                                            }
                                        }
                                    }


                                }
                                else  // PASO 5 - SI NO EXISTE CONTINUA CON EL PROCESO DE TIMBRADO
                                {
                                    DataTable results = facLabControler.TieneMercancias(segmento);
                                    //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                                    foreach (DataRow resl in results.Rows)
                                    {
                                        string totald = resl["total"].ToString();
                                        int num_var = Int32.Parse(totald);
                                        if (num_var > 0)
                                        {

                                            valida(segmento);

                                        }
                                    }
                                }

                            }
                        }
                    }

                    
                    
                        int total_items = dataz.package.total_items;
                }
                else
                {
                    InsertMercErrores(ship_id);
                }


            }
            //END SHIPMENTS
        }
        public  List<string> valida(string leg)
        {
            string compCarta = "";
            results.Clear();
            //PASO 6 - VALIDA EL TAMAÑO DEL SEGMENTO
            if (leg.Length > 0 && leg != "null" && leg != "")
            {
                try
                {
                    //VALIDO QUE TENGA MERCANCIA

                    List<string> validaCFDI = new List<string>();
                    //PASO 7 - VALIDA QUE ESTE OK LA CARTAPORTE
                    validaCFDI = sql.recuperaRegistros("exec sp_validaCFDICartaporte " + leg);
                    if (validaCFDI.Count > 0)
                    {
                        //PASO 8 - VALIDA QUE ESTE OK EL RESULTADO
                        if (validaCFDI[1].Contains("OK"))
                        {
                            //PASO 9 - CREA EL CUERPO DEL TXT
                            compCarta = sql.recuperaValor("exec sp_compCartaPorte " + leg);
                            if (compCarta.Length > 0)
                            {
                                tiposCfds();
                                words = Regex.Replace(compCarta, @"\r\n?|\n", "").Split('|');
                                iniciaDatos();
                                //PASO 10 - INGRESA PARA TIMBRAR LA CARTAPORTE
                                if (Cartaporte(leg, compCarta))
                                {
                                    //PASO 14 - ACTUALIZA EL ESTATUS A 2 - OK 
                                    results.Add("ok");//mostrar  }
                                    string tipom = "2";
                                    string titulo = "Carta porte generada: ";
                                    string mensaje = "Se genero correctamente la Carta porte.";
                                    //string mensaje = "Cartaporte timbrada con exito!!!";
                                    DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);

                                    //CON ESTO ACTUALIZAMOS EL ORDERHEADER 
                                    DataTable rorder = facLabControler.SelectLegHeader(leg);

                                    if (rorder.Rows.Count > 0)
                                    {
                                        foreach (DataRow reslo in rorder.Rows)
                                        {
                                            string rorderh = reslo["ord_hdrnumber"].ToString();
                                            DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                            string rfecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");
                                            DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                                            facLabControler.OrderHeader(rorderh, rfecha);
                                            DataTable getSeg = facLabControler.GetSegJr(leg);
                                            if (getSeg.Rows.Count > 0)
                                            {
                                                foreach (DataRow itemSeg in getSeg.Rows)
                                                {
                                                    string gbilto = itemSeg["billto"].ToString();
                                                    facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                                    WebForm1 muobject = new WebForm1();
                                                    muobject.RCPorte(leg);

                                                    
                                                }
                                            }
                                            //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                                        }
                                    }
                                    
                                    facLabControler.enviarNotificacion(leg,titulo, mensaje);
                                    //string msg = "Existoso: Se timbro correctamente la Carta porte:" + leg;
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Carta porte timbrada ', 'success');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                                    //Aqui actualizamos en estatus 

                                }
                                else
                                {
                                    results.Clear();
                                    results.Add("Error1");
                                    results.Add("Ver el historial de errores para mas información, copiar el error y reportar a TI.");
                                    string tipom = "3";
                                    string titulo = "Error en el segmento: ";
                                    //string mensaje = "Ver el historial de errores para mas información, copiar el error y reportar a TI.";
                                    DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                                    DataTable rorder = facLabControler.SelectLegHeaderOnly(leg);

                                    if (rorder.Rows.Count > 0)
                                    {
                                        foreach (DataRow reslo in rorder.Rows)
                                        {
                                            string rorderh = reslo["ord_hdrnumber"].ToString();
                                            //DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                            string rfecha = "null";
                                            //DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                                            //facLabControler.OrderHeader(rorderh, rfecha);
                                            DataTable getSeg = facLabControler.GetSegJr(leg);
                                            if (getSeg.Rows.Count > 0)
                                            {
                                                foreach (DataRow itemSeg in getSeg.Rows)
                                                {
                                                    string gbilto = itemSeg["billto"].ToString();
                                                    facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                                }
                                            }
                                            //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                                        }
                                    }

                                    string msg = "Error en el segmento:" + leg;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);


                                }
                            }
                            else
                            {
                                results.Clear();
                                results.Add("Error1");
                                results.Add("Error al generar carta porte.");//mostrar 
                                string tipom = "3";
                                string titulo = "Error en el segmento: ";
                                string mensaje = "Error al generar carta porte.";
                                DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                                DataTable rorder = facLabControler.SelectLegHeader(leg);

                                if (rorder.Rows.Count > 0)
                                {
                                    foreach (DataRow reslo in rorder.Rows)
                                    {
                                        string rorderh = reslo["ord_hdrnumber"].ToString();
                                        //DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                        string rfecha = "null";
                                        //DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                                        //facLabControler.OrderHeader(rorderh, rfecha);
                                        DataTable getSeg = facLabControler.GetSegJr(leg);
                                        if (getSeg.Rows.Count > 0)
                                        {
                                            foreach (DataRow itemSeg in getSeg.Rows)
                                            {
                                                string gbilto = itemSeg["billto"].ToString();
                                                facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                            }
                                        }
                                        //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                                    }
                                }

                                facLabControler.enviarNotificacion(leg, titulo, mensaje);
                                string msg = "Error al generar carta porte:" + leg;
                                ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                            }
                        }
                        else
                        {
                            // ERROR: YA EXISTE O YA ESTA TIMBRADO
                            results.Clear();
                            results.Add("Error");
                            results.Add("Error en la obtención de datos: \r\n" + validaCFDI[0]);//mostrar 
                            string tipom = "5";
                            string titulo = "Error en el segmento: ";
                            string mensaje = "Error en la obtención de datos:" + validaCFDI[0];

                            DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                            DataTable rorder = facLabControler.SelectLegHeaderOnly(leg);

                            if (rorder.Rows.Count > 0)
                            {
                                foreach (DataRow reslo in rorder.Rows)
                                {
                                    string rorderh = reslo["ord_hdrnumber"].ToString();
                                    //DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                    string rfecha = "null";
                                    //DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                                    //facLabControler.OrderHeader(rorderh, rfecha);
                                    DataTable getSeg = facLabControler.GetSegJr(leg);
                                    if (getSeg.Rows.Count > 0)
                                    {
                                        foreach (DataRow itemSeg in getSeg.Rows)
                                        {
                                            string gbilto = itemSeg["billto"].ToString();
                                            facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                        }
                                    }
                                    //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                                }
                            }
                            facLabControler.enviarNotificacion(leg, titulo, mensaje);
                            Rcartaporte.Value = validaCFDI[0];

                            string msg = "Error: en la obtención de datos:" + leg;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert()", true);
                        }
                    }
                    else
                    {
                        results.Clear();
                        results.Add("Error");
                        results.Add("Error al validar el segmento.");//mostrar 
                        string tipom = "3";
                        string titulo = "Error en el segmento: ";
                        string mensaje = "Error al validar el segmento.";
                        DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                        DataTable rorder = facLabControler.SelectLegHeaderOnly(leg);

                        if (rorder.Rows.Count > 0)
                        {
                            foreach (DataRow reslo in rorder.Rows)
                            {
                                string rorderh = reslo["ord_hdrnumber"].ToString();
                                //DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                string rfecha = "null";
                                //DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                                //facLabControler.OrderHeader(rorderh, rfecha);
                                DataTable getSeg = facLabControler.GetSegJr(leg);
                                if (getSeg.Rows.Count > 0)
                                {
                                    foreach (DataRow itemSeg in getSeg.Rows)
                                    {
                                        string gbilto = itemSeg["billto"].ToString();
                                        facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                    }
                                }
                                //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                            }
                        }

                        facLabControler.enviarNotificacion(leg, titulo, mensaje);
                        string msg = "Error al validar el segmento:" + leg;
                        ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                    }
                }
                catch (Exception)
                {
                    results.Clear();
                    results.Add("Error");
                    results.Add("Segmento invalido");
                    string tipom = "3";
                    string titulo = "Error en el segmento: ";
                    string mensaje = "Segmento invalido";
                    DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                    DataTable rorder = facLabControler.SelectLegHeaderOnly(leg);

                    if (rorder.Rows.Count > 0)
                    {
                        foreach (DataRow reslo in rorder.Rows)
                        {
                            string rorderh = reslo["ord_hdrnumber"].ToString();
                            //DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                            string rfecha = "null";
                            //DataTable uporder = facLabControler.UpdateOrderHeader(rorderh, rfecha);
                            //facLabControler.OrderHeader(rorderh, rfecha);
                            DataTable getSeg = facLabControler.GetSegJr(leg);
                            if (getSeg.Rows.Count > 0)
                            {
                                foreach (DataRow itemSeg in getSeg.Rows)
                                {
                                    string gbilto = itemSeg["billto"].ToString();
                                    facLabControler.InsertOrderReport(rorderh, leg, gbilto, tipom, rfecha);
                                }
                            }
                            //facLabControler.PullReportLiverded(rorderh,leg,rfecha);
                        }
                    }
                    facLabControler.enviarNotificacion(leg, titulo, mensaje);
                    string msg = "Segmento invalido:" + leg;
                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error ', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                }
            }
            else { results.Add("Error3"); }
            return results;
        }


        public static void tiposCfds()
        {
            var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/" + "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4" + "/tiposCfds");
            var response_ = (HttpWebResponse)request_.GetResponse();
            var responseString_ = new StreamReader(response_.GetResponseStream()).ReadToEnd();

            string[] separadas_ = responseString_.Split('}');

            foreach (string dato in separadas_)
            {
                if (dato.Contains("TDRXP"))
                {
                    string[] separadasSucursal_ = dato.Split(',');
                    foreach (string datoSuc in separadasSucursal_)
                    {
                        if (datoSuc.Contains("idSucursal"))
                        {
                            idSucursal = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];
                        }

                        if (datoSuc.Contains("id") && !datoSuc.Contains("idSucursal"))
                        {
                            idTipoFactura = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];
                        }
                    }
                }
            }
        }

        //PASO 11 - RECIBE EL SEGMENTO Y EL CUERPO DEL TXT
        public static bool Cartaporte(string leg, string strtext)
        {
            jsonFactura = "{\r\n\r\n  \"idTipoCfd\":" + "\"" + idTipoFactura + "\"";
            jsonFactura += ",\r\n\r\n  \"nombre\":" + "\"" + leg + ".txt" + "\"";
            jsonFactura += ",\r\n\r\n  \"idSucursal\":" + "\"" + idSucursal + "\"";
            //jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + Regex.Replace(strtext, @"\r\n?|\n", "") + "\"" + "\r\n\r\n}";
            jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + strtext + "\"" + "\r\n\r\n}";

            string folioFactura = "", serieFactura = "", uuidFactura = "", pdf_xml_descargaFactura = "", pdf_descargaFactura = "", xlm_descargaFactura = "", cancelFactura = "", error = "";
            string salida = "";

            try
            {
                //IdApiEmpresa = "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4";
                //PASO 12 - HACE UNA PETICION PUT A TRALIX PARA TIMBRAR LA CARTAPORTE
                var client = new RestClient("https://canal1.xsa.com.mx:9050/" + "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4" + "/cfdis");
                var request = new RestRequest(Method.PUT);

                request.AddHeader("cache-control", "no-cache");

                request.AddHeader("content-length", "834");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("Host", "canal1.xsa.com.mx:9050");
                request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");

                request.AddParameter("application/json", jsonFactura, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                string respuesta = response.StatusCode.ToString();
                //PASO 13 - AQUI VALIDA LA RESPUESTA DE TRALIX Y SI ES OK AVANZA Y SUBE AL FTP E INSERTA EL REGISTRO A VISTA_CARTA_PORTE
                if (respuesta == "BadRequest")
                {
                    string titulo = "Error en el segmento: ";
                    //string mensaje = "Error al validar el segmento.";
                    string merror = response.Content.ToString();
                    //DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                    facLabControler.enviarNotificacion(leg, titulo, merror);
                    return false;
                }
                string[] separadaFactura = response.Content.ToString().Split(',');

                List<string> erroes = new List<string>();

                for (int i = 0; i < 7; i++)
                {
                    try
                    {

                        error = separadaFactura[i].Replace("\\n", "").Replace("]}", "").Replace(@"\", "").Replace("\\t", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        erroes.Add(error);
                    }
                    catch (Exception)
                    {
                        erroes.Add("N/A");
                    }
                }



                foreach (string factura in separadaFactura)
                {
                    if (factura.Contains("errors") || factura.Contains("error"))
                    {

                        salida = "FALLA AL SUBIR";

                        DateTime fecha1 = DateTime.Now;
                        string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                        facLabControler.ErroresgeneradasCP(fechaFinal, leg, erroes[0], erroes[1], erroes[2], erroes[3], erroes[4], erroes[5], erroes[6]);
                        return false;
                    }
                    else
                    {
                        if (factura.Contains("folio"))
                        {
                            folioFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("serie"))
                        {
                            serieFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("uuid"))
                        {
                            uuidFactura = factura.Replace(factura.Substring(0, 4), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("pdfAndXmlDownload"))
                        {
                            pdf_xml_descargaFactura = factura.Replace(factura.Substring(0, 17), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("pdfDownload"))
                        {
                            pdf_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("xmlDownload") && !factura.Contains("pdfAndXmlDownload"))
                        {
                            xlm_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("cancellCfdi"))
                        {
                            cancelFactura = factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error1 = ex.Message;
            }

            string ftp = System.Web.Configuration.WebConfigurationManager.AppSettings["ftp"];
            if (ftp.Equals("Si"))
            {
                string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + leg + ".txt";
                UploadFile file = new UploadFile();
            }
            if (salida != "FALLA AL SUBIR")
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["activa"].Equals("Si"))
                {
                    //Modifica referencia
                    string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + leg.Trim();

                    DateTime fecha1 = Convert.ToDateTime(Fecha);
                    string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;
                    string origenn = "1";
                    facLabControler.generadas(folioFactura, serieFactura, uuidFactura, pdf_xml_descargaFactura, pdf_descargaFactura, xlm_descargaFactura, cancelFactura, leg, fechaFinal, Total, Moneda, RFC, origenn, Destino);
                    result.Add(folioFactura);
                    result.Add(serieFactura);
                    result.Add(uuidFactura);
                    result.Add(pdf_xml_descargaFactura);
                    result.Add(pdf_descargaFactura);
                    result.Add(xlm_descargaFactura);
                    result.Add(cancelFactura);
                    result.Add(leg);
                    result.Add(fechaFinal);
                    return true;
                }
                return true;
            }
            else
            {
                return false;//"Error al conectar al servicio XSA";
            }
        }
        public static void iniciaDatos()
        {
            Fecha = words[4].ToString();
            Subtotal = words[5].ToString();
            Totalimptrasl = words[6].ToString();
            Totalimpreten = words[7].ToString();
            Descuentos = words[8].ToString();
            Total = words[9].ToString();
            FormaPago = words[11].ToString();
            Condipago = words[12].ToString();
            MetodoPago = words[13].ToString();
            Moneda = words[14].ToString();
            RFC = words[22].ToString();
            CodSAT = words[39].ToString();
            IdProducto = words[43].ToString();
            Producto = "Viaje";
            Origen = "";// words[321].ToString();
            Destino = "";// words[322].ToString();

            result.Add(Fecha);
            result.Add(Subtotal);
            result.Add(Totalimptrasl);
            result.Add(Totalimpreten);
            result.Add(Descuentos);
            result.Add(Total);
            result.Add(FormaPago);
            result.Add(Condipago);
            result.Add(MetodoPago);
            result.Add(Moneda);
            result.Add(RFC);
            result.Add(CodSAT);
            result.Add(IdProducto);
            result.Add(Producto);
            result.Add(Origen);
            result.Add(Destino);
        }
        public static Hashtable generaActualizacion()
        {
            Hashtable datosTabla = conceptosFinales();
            Hashtable actualiza = new Hashtable();

            foreach (int item in datosTabla.Keys)
            {
                ArrayList list = (ArrayList)datosTabla[item];
                string tipoConcepto = list[3].ToString();
                double total = double.Parse(list[5].ToString());
                if (actualiza.ContainsKey(tipoConcepto))
                {
                    double val = double.Parse(actualiza[tipoConcepto].ToString());
                    actualiza[tipoConcepto] = val + total;
                }
                else
                {
                    actualiza.Add(tipoConcepto, total);
                }
            }
            return actualiza;
        }


        [WebMethod]
        public static object gettable()
        {
            List<CartaPorterest> lista = new List<CartaPorterest>();

            DataTable data = new DataTable();
            data = sql.ObtieneTabla("SELECT TOP 25 Folio, Serie, UUID, Pdf_xml_descarga, Pdf_descargaFactura, replace(xlm_descargaFactura,'}','') as xml_descargaFactura, replace(cancelFactura,'}','') as cancelFactura, LegNum, Fecha, Total, Moneda, RFC,Origen, Destino FROM VISTA_Carta_Porte ORDER BY FECHA DESC");
            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    lista.Add(new CartaPorterest(data.Rows[i][0].ToString(), data.Rows[i][1].ToString(), data.Rows[i][2].ToString(), "<a href=" + '\u0022' + "https://canal1.xsa.com.mx:9050" + data.Rows[i][3].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "ZIP" + '\u0022' + "/>" + "</a>", "<a href=" + '\u0022' + data.Rows[i][4].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "PDF" + '\u0022' + "/>" + "</a>", "<a href=" + '\u0022' + data.Rows[i][5].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "XML" + '\u0022' + "/>" + "</a>", "<button type=" + '\u0022' + "button" + '\u0022' + " OnClick=" + '\u0022' + "cancelCP('" + data.Rows[i][2].ToString() + "'" + ", '" + data.Rows[i][0].ToString() + "' )" + '\u0022' + ">" + "Cancelar" + "</button>", data.Rows[i][7].ToString(), data.Rows[i][8].ToString(), data.Rows[i][9].ToString(), data.Rows[i][10].ToString(), data.Rows[i][11].ToString(), data.Rows[i][12].ToString(), data.Rows[i][13].ToString()));
                }
            }
            object json = new { data = lista };
            return json;
        }

        public static Hashtable conceptosFinales()
        {
            table = new HtmlTable();
            Hashtable datos = new Hashtable();
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                TextBox cant = (TextBox)table.FindControl("" + i + "1");
                TextBox unidad = (TextBox)table.FindControl("" + i + "1");
                TextBox concepto = (TextBox)table.FindControl("" + i + "2");
                DropDownList tmp = (DropDownList)table.FindControl("" + i + "3");
                TextBox valor = (TextBox)table.FindControl("" + i + "4");
                TextBox importe = (TextBox)table.FindControl("" + i + "5");

                double cantidad = Math.Abs(double.Parse(cant.Text));

                //double cantidad = Double.Parse(cant.Text);

                ArrayList list = new ArrayList();
                list.Add(cantidad.ToString());
                list.Add(unidad.Text);
                list.Add(concepto.Text);
                list.Add(tmp.SelectedValue);
                list.Add(valor.Text);
                list.Add(importe.Text);

                if (datos.ContainsKey(tmp.Text))
                {
                    datos[i] = list;
                }
                else
                {
                    datos.Add(i, list);
                }
            }
            return datos;
        }
        public void InsertMerc(string Ai_orden, string id, string cate, string descript, int weight, string uwe, string quanti, string unitcode)
        {
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Insert_Api_MercadoL_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@Ai_orden", Ai_orden);
                    selectCommand.Parameters.AddWithValue("@id", id);
                    selectCommand.Parameters.AddWithValue("@cate", cate);
                    selectCommand.Parameters.AddWithValue("@descript", descript);
                    selectCommand.Parameters.AddWithValue("@weight", weight);
                    selectCommand.Parameters.AddWithValue("@uwe", uwe);
                    selectCommand.Parameters.AddWithValue("@quanti", quanti);
                    selectCommand.Parameters.AddWithValue("@unitcode", unitcode);

                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        public void InsertMercErrores(string ship_id)
        {
            string cadena = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Insert_Api_MercadoL_Errores_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 1000;
                    selectCommand.Parameters.AddWithValue("@ship_id", ship_id);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }

        public void RCPorte(string segmento)
        {
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


                        string msg = "Carta Porte generada: " + leg;
                        Rcartaporte.Value = msg;
                        ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Carta Porte generada ', 'success');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 100000)", true);
                        FolioBox.Text = Folio;
                        SerieBox.Text = Serie;
                        UUIDBox.Text = UUID;
                        ZipBox.NavigateUrl = Pdf_xml_descarga;
                        PdfBox.NavigateUrl = Pdf_descargaFactura;
                        xmlBox.NavigateUrl = xlm_descargaFactura;
                        card2.Visible = true;
                        card1.Visible = false;



                    }

                }
            }

        }
    }
}