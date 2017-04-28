<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="APT._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        
    <div class="text-center">
        <div class="jumbotron">
            <h1>BOOLA</h1>
            <h4>Everything is one click away</h4>
        </div>



        <div class="row">
            <div class="col-md-2">
            </div>
            <div class="col-md-8">
                <div class="inner-addon left-addon">
                    <i class="glyphicon glyphicon-search"></i>
                    <asp:TextBox class="form-control col-centered" type="text" ID="Textbox_Input" runat="server"></asp:TextBox>
                </div>
            </div>          
            <div class="col-md-2">
            </div>         
        </div>

        <br>

        <div class="row">
            <div class="col-md-4">
            </div>
            <div class="col-md-2">
                <asp:Button class="btn btn-block btn-default" ID="Btn_Search" runat="server" Text="Search" OnClick="Btn_Search_Click" />
            </div>
            <div class="col-md-2">
                <asp:Button class="btn btn-block btn-default" ID="Btn_ImageSearch" runat="server" Text="Image Search" OnClick="Btn_ImageSearch_Click" />
            </div>
            <div class="col-md-4">
            </div>
        </div>
    </div>

    <footer class="text-center bot">
        <div>
            <p>&copy; <%: DateTime.Now.Year %> - BOOLA</p>
        </div>                
    </footer>
            

</asp:Content>
