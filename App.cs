namespace oemLeads
{
    public class App
    {
        public static string Name => "svc.vw-rwil-leads-adapter";
        public static string ElasticSearch => "elasticsearch:url";
        public static string RwilEndpoint => "RwilEndpoint";
        public static string RwilAuthenticationEndpoint => "RwilAuthenticationEndpoint";
        public static string Rwil_host => RwilLeadsAdaptor_SetRwilHost();
        public static string Rwil_context => RwilLeadsAdaptor_SetRwilContext();
        public static string Rwil_client_id => RwilLeadsAdaptor_SetRwilClientID();
        public static string Rwil_client_secret => RwilLeadsAdaptor_SetRwilClientSecret();
        public static string Rwil_systemid => RwilLeadsAdaptor_SetRwilSystemID();
        public static string Rwil_consumername => RwilLeadsAdaptor_SetRwilConsumerName();
        public static string Rwil_partnerkey => RwilLeadsAdaptor_SetRwilPartnerKey();
        public static string Rwil_targetsystem => RwilLeadsAdaptor_SetRwilTargetSystem();
        public static string Keyloop_baseurl => RwilLeadsAdaptor_SetKeyloopBaseURL();
        public static string Keyloop_enteprise => RwilLeadsAdaptor_SetKeyloopEntepriseID();
        public static string Keyloop_store => RwilLeadsAdaptor_SetkeyloopStoreID();
        public static string Keyloop_client_id => RwilLeadsAdaptor_SetKeyloopClientID();
        public static string Keyloop_client_secret => RwilLeadsAdaptor_SetKeyloopClientSecret();
        public static string KeyloopLeads_URL => RwilLeadsAdaptor_SetKeyloopLeadsURL();
        public static string KeyloopLeads_IntegrationKey => RwilLeadsAdaptor_SetKeyloopLeadsIntegrationKey();
        public static string KeyloopLeads_DealerCode => RwilLeadsAdaptor_SetKeyloopLeadsDealerCode();
        public static string KeyloopLeads_User => RwilLeadsAdaptor_SetKeyloopLeadsUser();
        public static string KeyloopLeads_Password => RwilLeadsAdaptor_SetKeyloopLeadsPassword();
        public static string RwilLeadsAdaptor_SetRwilHost()
        // location parameter
        {
            return "rwil-qa.volkswagenag.com";
        }
        public static string RwilLeadsAdaptor_SetRwilContext()
        // location parameter
        {
            return "rwil/gedai";
        }
        public static string RwilLeadsAdaptor_SetRwilClientID()
        // enterprise config
        {
            return "ZAF996-Autoline-DRIVE-DMS-Q";
        }
        public static string RwilLeadsAdaptor_SetRwilClientSecret()
        // enterprise config
        {
            return "ghVThqVlpUa83Pmkr2Ghs73yTZ69IyEI2d3g";
        }
        public static string RwilLeadsAdaptor_SetRwilSystemID()
        // location parameter
        {
            return "ZAF996-Autoline-DRIVE-DMS-Q";
        }
        public static string RwilLeadsAdaptor_SetRwilConsumerName()
        // location parameter config
        {
            return "ZAF996-Autoline-DRIVE-DMS-Q";
        }
        public static string RwilLeadsAdaptor_SetRwilPartnerKey()
        // Enterprise config
        {
            return "ZAF09002V";
        }
        public static string RwilLeadsAdaptor_SetRwilTargetSystem()
        // location parameter
        {
            return "Product_SLI_QA";
        }
        public static string RwilLeadsAdaptor_SetKeyloopBaseURL()
        {
            return "api.af-stage.keyloop.io";
        }
        public static string RwilLeadsAdaptor_SetKeyloopEntepriseID()
        {
            return "31981";
        }
        public static string RwilLeadsAdaptor_SetkeyloopStoreID()
        {
            return "44014796-BR0001";
        }
        public static string RwilLeadsAdaptor_SetKeyloopClientID()
        {
            return "QZusCMlg42fU7m50CSg9AIAxI0G7yTpd";
        }
        public static string RwilLeadsAdaptor_SetKeyloopClientSecret()
        {
            return "AAtmfq1YLH1HugHs";
        }
        public static string RwilLeadsAdaptor_SetKeyloopLeadsURL()
        // Enterprise Config
        {
            return "secure2.live-lead.com";
        }
        public static string RwilLeadsAdaptor_SetKeyloopLeadsIntegrationKey()
        // Enterprise config
        {
            return "05537856-01b2-4f82-9e4e-afd500f002b1";
        }
        public static string RwilLeadsAdaptor_SetKeyloopLeadsDealerCode()
        // Enterprise Config
        {
            return "octanevwsa";
        }
        public static string RwilLeadsAdaptor_SetKeyloopLeadsUser()
        // Enterprise Config
        {
            return "keyloopplatform.integration@keyloop.com";
        }
        public static string RwilLeadsAdaptor_SetKeyloopLeadsPassword()
        // Enterprise Config
        {
            return "qFRy9Ce4hgpCJPcS";
        }

    }
}
