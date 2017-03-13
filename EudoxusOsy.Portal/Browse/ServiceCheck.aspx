<%@ Page Title="" Language="C#" MasterPageFile="~/Browse/Browse.Master" AutoEventWireup="true" CodeBehind="ServiceCheck.aspx.cs" Inherits="TelecomOffers.Portal.Browse.ServiceCheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="./ServiceProxy.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            endpoint = "http://" + location.host + "/api/";

            $('#Services').change(function () {
                if ($('#Services option:selected').attr('value') == 'GET') {
                    $("#tbxID").show();
                    $("#tbxNumber").hide();
                    $("#tbxJSon").hide();
                }
                else if ($('#Services option:selected').attr('value') == 'POST') {
                    $("#tbxID").hide();
                    $("#tbxNumber").hide();
                    $("#tbxJSon").show();
                }
                else if ($('#Services option:selected').attr('value') == 'PUT') {
                    $("#tbxID").hide();
                    $("#tbxNumber").hide();
                    $("#tbxJSon").show();
                }
            });

            $("#btnLogin").click(function () {
                var cred = {
                    Username: $("#tbxUsername").val(),
                    Password: $("#tbxPassword").val()
                };
                ServiceProxy.Login(cred,
                    function (msg) {
                        if (msg.Success == true) {
                            $("#preLogin").hide();
                            $("#txtAccessToken").html(msg.Result.AccessToken);
                            auth = msg.Result.AccessToken;
                            $("#postLogin").show();
                        }
                    },
                    function (error) {
                        alert(JSON.stringify(error));
                    });
            });

            $("#btnSend").click(function () {


                if ($('#Services option:selected').attr('value') == 'GET') {
                    window["ServiceProxy"][$("#Services option:selected").text()]($("#tbxID").val(),
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.StatusCode + ' - ' + msg.StatusMessage));
                        }
                    },
                    function (error) {
                        $("#tbxResult").html(JSON.stringify(error));
                    });
                }
                else if ($('#Services option:selected').attr('value') == 'GET2') {
                    window["ServiceProxy"][$("#Services option:selected").text()]($("#tbxID").val(), $("#tbxNumber").val(),
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.StatusCode + ' - ' + msg.StatusMessage));
                        }
                    },
                    function (error) {
                        $("#tbxResult").html(JSON.stringify(error));
                    });
                }
                else if ($('#Services option:selected').attr('value') == 'POST') {
                    var json;
                    if ($("#tbxJSon").val() != '') {
                        json = jQuery.parseJSON($("#tbxJSon").val());
                    }
                    else {
                        json = null;
                    }
                    window["ServiceProxy"][$("#Services option:selected").text()](json,
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.StatusCode + ' - ' + msg.StatusMessage));
                        }
                    },
                    function (error) {
                        alert(JSON.stringify(error));
                    });
                }
                else if ($('#Services option:selected').attr('value') == 'PUT') {
                    var json;
                    if ($("#tbxJSon").val() != '') {
                        json = jQuery.parseJSON($("#tbxJSon").val());
                    }
                    else {
                        json = null;
                    }
                    window["ServiceProxy"][$("#Services option:selected").text()](json,
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.StatusCode + ' - ' + msg.StatusMessage));
                        }
                    },
                    function (error) {
                        alert(JSON.stringify(error));
                    });
                }

            });
        });

    </script>

    <h1>Service Tester</h1>
    <br />
    <br />
    <div id="preLogin">
        <input type="text" id="tbxUsername" title="Όνομα" placeholder="Όνομα χρήστη..." />
        <input type="password" id="tbxPassword" title="Κωδικός" placeholder="Κωδικός πρόσβασης..." />
        <input type="button" id="btnLogin" value="Login" />
    </div>
    <div id="postLogin" style="display: none">
        Access Token:
                    <br />
        <asp:TextBox ID="txtAccessToken" ClientIDMode="Static" ReadOnly="true" Style="width: 50%" runat="server" Rows="3" TextMode="MultiLine" />
        <br />
        <br />
        <select id="Services">
            <option></option>            
            <option value="GET">IsOfferPublished</option>
            <option value="POST">IsOfferPublishedPOST</option>            
        </select>
        <input type="text" id="tbxID" style="display: none" placeholder="ID" />
        <input type="text" id="tbxNumber" style="display: none" placeholder="StudentNumber" />
        <asp:TextBox ID="tbxJSon" ClientIDMode="Static" Style="width: 50%" runat="server" Rows="3" TextMode="MultiLine" placeholder="Properties" />
        <input type="button" id="btnSend" value="Send" />

        <br />
        <br />
        <br />

        <asp:TextBox ID="tbxResult" ClientIDMode="Static" Style="width: 100%" runat="server" Rows="3" ReadOnly="true" TextMode="MultiLine" placeholder="Result" />
        <br />
    </div>
</asp:Content>
