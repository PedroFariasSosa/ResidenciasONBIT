using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaEPAS
{
    class CarroCompra
    {
        private long id;
        private Cliente cliente;
        private CarroCompraEntregaDomicilio carrocompraentregadomicilio;
        private CarroCompraPago carrocomprapago;


        public long Id { get => id; set => id = value; }
        public Cliente Cliente { get => cliente; set => cliente = value; }
        internal CarroCompraEntregaDomicilio Carrocompraentregadomicilio { get => carrocompraentregadomicilio; set => carrocompraentregadomicilio = value; }
        internal CarroCompraPago Carrocomprapago { get => carrocomprapago; set => carrocomprapago = value; }
    }
}
