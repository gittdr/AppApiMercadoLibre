<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CARGAR_EXCEL.WebForm1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TDR | TI APPS</title>
    
    <link rel="icon" href="Models/icon.png">
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" ></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
    
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
     <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/bootbox.js/5.5.2/bootbox.min.js"></script>--%>
    <script type="text/javascript" src='https://cdn.jsdelivr.net/sweetalert2/6.3.8/sweetalert2.min.js'> </script>
        <link rel="stylesheet" href='https://cdn.jsdelivr.net/sweetalert2/6.3.8/sweetalert2.min.css'
            media="screen" />
    <script src="https://kit.fontawesome.com/789a3ce2b4.js" crossorigin="anonymous"></script>
    <style>
        .mitabla {
            width :100%
        }
        ::-webkit-scrollbar {
  width: 10px;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px grey; 
  border-radius: 10px;
}
 
/* Handle */
::-webkit-scrollbar-thumb {
 background: grey;
  border-radius: 10px;
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background-image: linear-gradient(to bottom right, #00315f,#f7be31 );
}

        body {
            margin: 0;
            background: #000000;
            font-family: Arial;
        }

        nav {
            position: fixed;
            width: 100%;
            max-width: 300px;
            bottom: 0;
            top: 0;
            display: block;
            min-height: 300px;
            height: 100%;
            color: #fff;
            opacity: 0.8;
            transition: all 300ms;
            -moz-transition: all 300ms;
            -webkit-transition: all 300ms;
        }

            nav .vertical-menu hr {
                opacity: 0.1;
                border-width: 0.5px;
            }

            nav ul {
                width: 90%;
                padding-inline-start: 0;
                margin: 10px;
                height: calc(100% - 20px);
            }

            nav .vertical-menu-logo {
                padding: 10px;
                font-size: 1.3em;
                position: relative
            }

                nav .vertical-menu-logo .open-menu-btn {
                    width: 30px;
                    height: max-content;
                    position: absolute;
                    display: block;
                    right: 20px;
                    top: 0;
                    bottom: 0;
                    margin: auto;
                    cursor: pointer;
                }

                    nav .vertical-menu-logo .open-menu-btn hr {
                        margin: 5px 0
                    }

            nav li {
                list-style: none;
                padding: 10px 10px;
                cursor: pointer;
            }

                /*nav li:hover {
                    -webkit-transition-delay:0s;
                   -webkit-transition-duration:0.5s;
                   -webkit-transition-property:all;
                   -webkit-transition-timing-function:ease;
                   background-color:#95a5a6;
                   border-start-end-radius:10px !important;
                }*/

                .stiloli:hover {
                     -webkit-transition-delay:0s;
                   -webkit-transition-duration:0.5s;
                   -webkit-transition-property:all;
                   -webkit-transition-timing-function:ease;
                   background-color:#95a5a6;
                   border-start-end-radius:10px !important;
                }
                .stilolic:hover {
                    -webkit-transition-delay:0s;
                   -webkit-transition-duration:0.5s;
                   -webkit-transition-property:all;
                   -webkit-transition-timing-function:ease;
                     transform: scale(1.5);
                }

                nav li#user-info {
                    position: absolute;
                    bottom: 0;
                    width: 80%;
                }

                    nav li#user-info > span {
                        display: block;
                        float: right;
                        font-size: 0.9em;
                        position: relative;
                        opacity: 0.6;
                    }

                        nav li#user-info > span:after {
                            content: '';
                            width: 12px;
                            height: 12px;
                            display: block;
                            position: absolute;
                            background: #13ff13;
                            left: -20px;
                            top: 0;
                            bottom: 0;
                            margin: auto;
                            border-radius: 50%;
                        }

        .content-wrapper {
            width: calc(100% - 300px);
            height: 100%;
            position: fixed;
            background: #fff;
            left: 300px;
            padding: 20px;
            
        }

        .closed-menu .content-wrapper {
            width: 100%;
            left: 50px;
        }

        .content-wrapper {
            transition: all 300ms;
        }

        .vertical-menu-wrapper .vertical-menu-logo div {
            transition: all 100ms;
        }

        .closed-menu .vertical-menu-wrapper .vertical-menu-logo div {
            margin-left: -100px;
        }

        .vertical-menu-wrapper .vertical-menu-logo .open-menu-btn {
            transition: all 300ms;
        }

        .closed-menu .vertical-menu-wrapper .vertical-menu-logo .open-menu-btn {
            left: 7px;
            right: 100%;
        }

        .closed-menu .vertical-menu-wrapper ul, .closed-menu .vertical-menu-wrapper hr {
            margin-left: -300px;
        }

        .vertical-menu-wrapper ul, .vertical-menu-wrapper hr {
            transition: all 100ms;
        }

        .content-wrapper {
            background: #ebebeb;
        }

        .content {
            width: 100%;
            min-height: 90%;
            background: #fff;
            border-radius: 10px;
            padding: 30px;
            z-index: 1900;
        }
            
    </style>
    <script>
        $(document).ready(function () {
            $('.open-menu-btn').on('click', function () {
                if ($('body').hasClass('closed-menu')) {
                    $('body').removeClass('closed-menu');
                } else $('body').addClass('closed-menu');
            });
        });
    </script>
    <script type="text/javascript">
        
        function RCalert() {
            var verc = document.getElementById('card2');
            verc.style.display = 'block';
            <%--var divv = document.getElementById('<%=Rcartaporte.ClientID%>').value;
            
            
            swal({
                title: '<h1><i style="color:#a5dc86;font-size:80px;" class="fa fa-check-circle" aria-hidden="true"></i></h1>',
                icon: 'success',
                html: '<div class="alert alert-success" role="alert">' + divv+'</div>',
                showCloseButton: false,
                showCancelButton: false,
                focusConfirm: false
            });--%>
            return true;
        }
        

    </script>
    <script type="text/javascript">

        function Showalert() {

            var divv = document.getElementById('<%=Rcartaporte.ClientID%>').value;
            
            
            swal({
                title: '<h1><i style="color:#f27474;font-size:80px;" class="fa fa-times-circle-o" aria-hidden="true"></i></h1>',
                icon: 'success',
                html: '<div class="alert alert-danger" role="alert">' + divv+'</div>',
                showCloseButton: false,
                showCancelButton: false,
                focusConfirm: false
            });
            return true;
        }
        function Showalert2() {
            var divv = document.getElementById('<%=HiddenField1.ClientID%>').value;
            swal({
                title: '<h1><i style="color:#f27474;font-size:80px;" class="fa fa-times-circle-o" aria-hidden="true"></i></h1>',
                icon: 'success',
                html: '<div class="alert alert-danger" role="alert">' + divv + '</div>',
                showCloseButton: false,
                showCancelButton: false,
                focusConfirm: false
            });
            return true;
        }

    </script>
    
    
</head>
<body>
    <nav class="vertical-menu-wrapper overflow-auto">
        <div class="vertical-menu-logo">
            <ul class="vertical-menu">
             <li>

                <img class="img-fluid" src="Models/logo.png" />
                

            </li></ul>
            <span class="open-menu-btn"><hr style="background-color:gray"><hr style="background-color:gray"><hr style="background-color:gray"></span>
        </div>
        <ul class="vertical-menu">
             <li>

                <b class="nav-link text-light" style="font-size:25px !important; text-align:center !important">Generar Carta Porte</b>
                

            </li>
            <li style="text-light;text-align:center !important;" class="mt-5"><small>2019 Copyright <br /> &copy; TDR Soluciones Logísticas</small></li>
        </ul>

    </nav>
    <div class="content-wrapper" style="overflow:scroll">
        <div class="content shadow-lg">
              <form id="form1" runat="server">
         <div class="container-fluid mt-4">
                 <div class="card" runat="server" id="card1">
                  <div class="card-header">
                    Generar Carta Porte
                  </div>
                  <div class="card-body">
                    <div class="row">
                        <div class="form-group col-md-6">
                            <label for="NOrden"># Orden</label>
                                    <asp:TextBox ID="NOrden" CssClass="form-control-file" runat="server" required="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFielValidator" ControlToValidate="NOrden" Display="Dynamic" ForeColor="Red" SetFocusOnError="True">* Campo requerido</asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="Segmento">Segmento</label>
                                    <asp:TextBox ID="Segmento" CssClass="form-control-file" runat="server" required="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="RequiredFielValidator" ControlToValidate="Segmento" Display="Dynamic" ForeColor="Red" SetFocusOnError="True">* Campo requerido</asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="NOrden">RouteID</label>
                                    <asp:TextBox ID="RouteID" CssClass="form-control-file" runat="server" required="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="RequiredFielValidator" ControlToValidate="RouteID" Display="Dynamic" ForeColor="Red" SetFocusOnError="True">* Campo requerido</asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-md-6">
                            <label for="NOrden">FacilityID</label>
                                    <asp:TextBox ID="FacilityID" CssClass="form-control-file" runat="server" required="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="RequiredFielValidator" ControlToValidate="FacilityID" Display="Dynamic" ForeColor="Red" SetFocusOnError="True">* Campo requerido</asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-12">
                            <div class="form-row">
                                
                                
                                <div class="form-group col-sm-12">
                                  <asp:Button ID="Button1" runat="server" Text="Registrar" CssClass="btn btn-block btn-success mt-4" OnClick="Button1_Click" />
                                </div>
                            </div>
                        </div>
                        
                        
                               
                               
                        <hr />
                        
                                
                                <asp:HiddenField ID="Rcartaporte"  runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="HiddenField1"  runat="server"></asp:HiddenField>       

                            
                            
                        

                    </div>
                      
                   
                  </div>

                </div>
             <br />
             <br />
             <div class="card" runat="server" id="card2">
                  <div class="card-header">
                    Carta Porte Registrada
                  </div>
                  <div class="card-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="alert alert-light" role="alert" style="width:100%">
                                    <table class="table">
                                      <thead>
                                        <tr>
                                          <th scope="col">Folio</th>
                                          <th scope="col">Serie</th>
                                          <th scope="col">UUID</th>
                                          <th scope="col">Descarga ZIP</th>
                                          <th scope="col">Descarga PDF</th>
                                          <th scope="col">Descarga XML</th>
                                        </tr>
                                      </thead>
                                      <tbody>
                                        <tr>
                                          <th scope="row">
                                              <asp:Label ID="FolioBox" CssClass="label" runat="server"></asp:Label>
                                          </th>
                                          <td><asp:Label ID="SerieBox" CssClass="label" runat="server"></asp:Label></td>
                                          <td><asp:Label ID="UUIDBox" CssClass="label" runat="server" ></asp:Label></td>
                                          <td><asp:HyperLink CssClass="btn btn-success" ID="ZipBox"  runat="server" Text="Descargar"><i class="fa fa-arrow-circle-down" aria-hidden="true"> ZIP</i></asp:HyperLink></td>
                                          <td><asp:HyperLink CssClass="btn btn-success" ID="PdfBox"  runat="server" Text="Descargar"><i class="fa fa-arrow-circle-down" aria-hidden="true"> PDF</i></asp:HyperLink></td>
                                          <td><asp:HyperLink ID="xmlBox" CssClass="btn btn-success" runat="server" Text="Descargar"><i class="fa fa-arrow-circle-down" aria-hidden="true"> XML</i></asp:HyperLink></td>
                                        </tr>
                                      </tbody>
                                        <tfoot>
                                            <tr>
                                          <th scope="col">Folio</th>
                                          <th scope="col">Serie</th>
                                          <th scope="col">UUID</th>
                                          <th scope="col">Descarga ZIP</th>
                                          <th scope="col">Descarga PDF</th>
                                          <th scope="col">Descarga XML</th>
                                           </tr>
                                        </tfoot>
                                    </table>
                                  
                                </div>
                            </div>
                        </div>

                    </div>
                      
                   
                  </div>

                </div>
        </div>

        
        <%--<div>
            
            <br />
            <br />
           
        </div>
        <br />
        <div>
            <asp:Label ID="lblrespuesta" runat="server"></asp:Label>
        </div>--%>
        
    </form>
        </div>
    </div>
 
</body>
</html>
