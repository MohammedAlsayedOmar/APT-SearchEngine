<%@ Page Title="Text Search Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchResults.aspx.cs" Inherits="APT.SearchResults" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="row">
        <div class="col-md-8">
            <div class="inner-addon right-addon">
                <i class="glyphicon glyphicon-search"></i>
                <asp:TextBox class="form-control col-centered" type="text" ID="Textbox_Input" runat="server"></asp:TextBox>
            </div>
        </div>          
        <div class="col-md-2">
            <asp:Button class="btn btn-block btn-default" ID="BtnSearchAgain" runat="server" Text="Search" OnClick="BtnSearchAgain_Click"  Visible="true"/>
        </div>     
        <div class="col-md-2">
            <asp:Button class="btn btn-block btn-default" ID="Btn_ImageSearch" runat="server" Text="Switch to Image" OnClick="Btn_ImageSearch_Click" TabIndex="1" />
        </div>    
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Label ID="Label_Text" runat="server" Text="Displaying Search Results for: "></asp:Label>
            <asp:Label ID="Label_Data" runat="server" Text="Temp"></asp:Label>
        </div>         
    </div>
    <hr />

    <div class="container">
        <asp:PlaceHolder ID="LinksPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <hr />
    <div class="row center-block text-center">
        <div class ="col-md-4"></div>               
        <div class ="col-md-1">
             <a id="arrowLeft" runat="server" class="glyphicon glyphicon-chevron-left"></a>
        </div>
        <div class ="col-md-2">
            <p id="pageNum" runat="server"> 1 </p>
        </div>
        <div class ="col-md-1">
            <a id="arrowRight" runat="server" class="glyphicon glyphicon-chevron-right"></a>
        </div>
        <div class ="col-md-4"></div>
    </div>


</asp:Content>
