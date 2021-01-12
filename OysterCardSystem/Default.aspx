<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OysterCardSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <div class="row">
            <div class="col-md-7">
                <h3>Your current balance is       
            <asp:Label runat="server" ID="lblCurrentBal" CssClass="text-primary"></asp:Label>
                </h3>
            </div>
            <div class="col-md-3">

                <div class="form-group">
                    <label for="txtAmount">(£) Amount</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                        ControlToValidate="txtAmount" runat="server"
                        ErrorMessage="Only Numbers allowed"
                        ValidationExpression="\d+"
                        CssClass="text-danger">
                    </asp:RegularExpressionValidator>
                </div>

                <asp:Button ID="btnTopupCard" runat="server" CssClass="btn btn-primary" Text="Top Up Oyster Card" OnClick="btnTopupCard_Click" />
            </div>
        </div>
    </div>

    <div class="jumbotron">
        <h3>Journey Simulation</h3>
        <hr />
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label for="ddJourneyType">Journey Type</label>
                    <asp:DropDownList ID="ddJourneyType" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1" Text="Tube" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Bus"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label for="ddFromPoint">Start Station</label>
                    <asp:DropDownList ID="ddFromPoint" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="holburn" Text="Holburn"></asp:ListItem>
                        <asp:ListItem Value="earlscourt" Text="Earl's Court"></asp:ListItem>
                        <asp:ListItem Value="hammersmith" Text="Hammersmith"></asp:ListItem>
                        <asp:ListItem Value="chelsea" Text="Chelsea"></asp:ListItem>
                        <asp:ListItem Value="wimbledon" Text="Wimbledon"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label for="ddEndPoint">End Station</label>
                    <asp:DropDownList ID="ddEndPoint" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="holburn" Text="Holburn"></asp:ListItem>
                        <asp:ListItem Value="earlscourt" Text="Earl's Court"></asp:ListItem>
                        <asp:ListItem Value="hammersmith" Text="Hammersmith"></asp:ListItem>
                        <asp:ListItem Value="chelsea" Text="Chelsea"></asp:ListItem>
                        <asp:ListItem Value="wimbledon" Text="Wimbledon"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:Button ID="btnJourneyFnished" runat="server" CssClass="btn btn-primary m-t-20" Text="Submit" OnClick="btnJourneyFnished_Click" />
                </div>
            </div>
        </div>
        <blockquote class="blockquote text-center">
            <asp:Label ID="lblErr" runat="server" CssClass="label-danger"></asp:Label>
        </blockquote>
    </div>
    <hr />
    <div class="container-fluid">
        <h3>Journey History</h3>
        <div class="table-responsive-md">
            <table class="table table-sm">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Initial Card balance</th>
                        <th scope="col">Journey Type</th>
                        <th scope="col">Start Station</th>
                        <th scope="col">End Station</th>
                        <th scope="col">Charges</th>
                        <th scope="col">Final Card Balance</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptCardLogs" runat="server">
                        <ItemTemplate>
                            <tr>
                                <th scope="row"><%# Eval("sno") %></th>
                                <td>£<%# Eval("initialCardBal") %></td>
                                <td><%# Eval("journeytype") %></td>
                                <td><%# Eval("startpoint") %></td>
                                <td><%# Eval("endpoint") %></td>
                                <td>£<%# Eval("charges") %></td>
                                <td>£<%# Eval("finalcardbal") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

        </div>
    </div>

    <script type="text/javascript"> 

        function validateJourneyType() {

            var ddlvalue = $("#<%=ddJourneyType.ClientID %>").val();
            if (ddlvalue == '1') {

                $("#<%=ddFromPoint.ClientID %> option[value='chelsea']").attr("disabled", "disabled");
                $("#<%=ddEndPoint.ClientID %> option[value='chelsea']").attr("disabled", "disabled");

                $('#<%=ddFromPoint.ClientID %>').val('0');
                $('#<%=ddEndPoint.ClientID %>').val('0');
            }
            else if (ddlvalue == '2') {

                $("#<%=ddFromPoint.ClientID %> option[value='chelsea']").removeAttr("disabled");
                $("#<%=ddEndPoint.ClientID %> option[value='chelsea']").removeAttr("disabled");

            }
        }

        $(document).ready(function () {

            $("#<%=ddJourneyType.ClientID %>").on('change', function () {

                validateJourneyType();
            });

            validateJourneyType();

        });
    </script>
</asp:Content>
