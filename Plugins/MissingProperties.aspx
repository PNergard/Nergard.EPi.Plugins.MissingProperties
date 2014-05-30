<%@ Page Language="c#" CodeBehind="MissingProperties.aspx.cs" AutoEventWireup="False" Inherits="Nergard.EPi.Plugins.MissingProperties" Title="Missing Propertis" %>
<%@ Register TagPrefix="EPiServerUI" Namespace="EPiServer.UI.WebControls" assembly="EPiServer.UI" %>

<asp:content contentplaceholderid="MainRegion" runat="server">
    <div class="epi-formArea" ID="Pagetypes" runat="server">
        <div class="epi-size25">
            <div>
                <asp:GridView
                ID="PropertiesViewControl"
                runat="server" 
                AutoGenerateColumns="false" >
                <Columns>
                    <asp:TemplateField HeaderText="Type" ItemStyle-Wrap="false">                
                        <ItemTemplate>
			                <b><%#Item.Heading %></b>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="TypeName" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <%#Item.TypeName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Property Name" ItemStyle-Wrap="false">                
                        <ItemTemplate>
                            <%#Item.Name%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ItemStyle-Wrap="false">                
                        <ItemTemplate>
                            <asp:CheckBox id="box" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" ItemStyle-Wrap="false" visible="false">                
                        <ItemTemplate>
                            <asp:Label id="typeid" Text="<%#Item.id %>" runat="server" /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            </div>
            <div class="epi-buttonContainer">
                <EPiServerUI:ToolButton id="Save" DisablePageLeaveCheck="true" OnClick="Delete" runat="server" SkinID="Save" text="Delete" ToolTip="Delete" />
            </div>
        </div>
    </div>
</asp:content>
