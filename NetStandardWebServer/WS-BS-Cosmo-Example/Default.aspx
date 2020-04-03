<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WS_BS_Cosmo_Example._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <asp:FileUpload ID="FileUpload" runat="server" />
        <br />
        <asp:Button ID="UploadImageButton" runat="server" OnClick="UploadImageButton_Click" Text="Upload Image" />
        <br />
        <br />
        <asp:Label ID="Status" runat="server" Text="" />
        <br />
</asp:Content>
