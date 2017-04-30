<%@ Page Title="Image Search Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchImageResults.aspx.cs" Inherits="APT.SearchImageResults" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="row">
        <div class="col-md-8">
            <div class="inner-addon left-addon">
                <i class="glyphicon glyphicon-search"></i>
                <asp:TextBox class="form-control col-centered" type="text" ID="Textbox_Input" runat="server"></asp:TextBox>
            </div>
        </div>          
        <div class="col-md-2">
            <asp:Button class="btn btn-block btn-default" ID="BtnSearchAgain" runat="server" Text="Search" OnClick="BtnSearchAgain_Click"  Visible="true"/>
        </div>     
        <div class="col-md-2">
            <asp:Button class="btn btn-block btn-default" ID="Btn_Search" runat="server" Text="Switch to Text" OnClick="Btn_Search_Click"/>
        </div>    
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Label ID="Label_Text" runat="server" Text="Displaying Image Results for: "></asp:Label>
            <asp:Label ID="Label_Data" runat="server" Text="Temp"></asp:Label>
        </div>        
    </div>
    <hr />

    <asp:PlaceHolder ID="ImagesPlaceHolder" runat="server"></asp:PlaceHolder>

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
