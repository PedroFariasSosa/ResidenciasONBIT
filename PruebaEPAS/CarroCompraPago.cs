using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaEPAS
{
    enum TipoMedioPago
    {
        Tarjetacreditolinea = 0,
        Tarjetadebitolinea = 1
    }

    enum Estatus
    {
        Aceptado = 0,
        Rechazado = 1,
        Porcobrar = 3
    }

    enum CodigoRespuesta
    {
        Aceptado = 1,
        TarjetaDeclinada = 2,
        TarjetaExpirada = 3,
        TarjetaSinFondos =4,
        TarjeraRobada = 5,
        TarjetaRechazada = 6,
        OperacionNoPermitida = 7,
        TarjetaNoSoportadaenLinea = 8,
        TarjetaReportada = 9,
        TarjetaRestringida = 10,
        TarjetaRetenida = 11,
        SolicitarAutorizacion = 12,
        TarjetaDeclinadaPorCVV = 13
    }
    class CarroCompraPago
    {
        private TipoMedioPago tipomediopago;
        private float montototal;
        private String currency;
        private DateTimeOffset fechaUTCtransaccion;
        private Estatus estatus;
        private String merchantsource;
        private CodigoRespuesta codigorespuesta;
        private String tarjetapan;
        private String holdername;
        private String tarjetamonth;
        private String tarjetayear;
        private DateTimeOffset fechacobroUTC;


        public TipoMedioPago Tipomediopago { get => tipomediopago; set => tipomediopago = value; }
        public float Montototal { get => montototal; set => montototal = value; }
        public string Currency { get => currency; set => currency = value; }
        public DateTimeOffset FechaUTCtransaccion { get => fechaUTCtransaccion; set => fechaUTCtransaccion = value; }
        public Estatus Estatus { get => estatus; set => estatus = value; }
        public string Merchantsource { get => merchantsource; set => merchantsource = value; }
        public CodigoRespuesta Codigorespuesta { get => codigorespuesta; set => codigorespuesta = value; }
        public String Tarjetapan { get => tarjetapan; set => tarjetapan = value; }
        public string Holdername { get => holdername; set => holdername = value; }
        public string Tarjetamonth { get => tarjetamonth; set => tarjetamonth = value; }
        public string Tarjetayear { get => tarjetayear; set => tarjetayear = value; }
        public DateTimeOffset FechacobroUTC { get => fechacobroUTC; set => fechacobroUTC = value; }
    }
}
