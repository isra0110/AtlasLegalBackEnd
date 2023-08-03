using Abp.Data;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.UI;
using Atlas.Legal.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Atlas.Legal
{
    public class CatalogosRepository : LegalRepositoryBase<Siniestro, int>, ICatalogosRepository
    {
        private readonly Dictionary<int, string> _listaCatalogos = new Dictionary<int, string>();

        public CatalogosRepository(IDbContextProvider<LegalDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
            : base(dbContextProvider, transactionProvider)
        {
            _listaCatalogos.Add(1, "CatDelito");
            _listaCatalogos.Add(2, "CatAgravante");
            _listaCatalogos.Add(3, "CatAutoridadConoce");
            _listaCatalogos.Add(4, "CatCausa");
            _listaCatalogos.Add(5, "CatCircunstancia");
            _listaCatalogos.Add(6, "CatDeslindResponsabilidad");
            _listaCatalogos.Add(7, "CatEstado");
            _listaCatalogos.Add(8, "CatEstapaProcesal");
            _listaCatalogos.Add(9, "CatEstatus");
            _listaCatalogos.Add(10, "CatLiberadoDetenido");
            _listaCatalogos.Add(11, "CatMesTurnado");
            _listaCatalogos.Add(12, "CatOficinaRegional");
            _listaCatalogos.Add(13, "CatPagoRecuperacion");
            _listaCatalogos.Add(14, "CatParteAccidente");
            _listaCatalogos.Add(15, "CatProveedor");
            _listaCatalogos.Add(16, "CatQuienFallece");
            _listaCatalogos.Add(17, "CatResponsabilidad");
            _listaCatalogos.Add(18, "CatSeguroContratado");
            _listaCatalogos.Add(19, "CatSucursalSegurosAtlas");
            _listaCatalogos.Add(20, "CatTipoLesionados");
            _listaCatalogos.Add(21, "CatTipoPersona");
            _listaCatalogos.Add(22, "CatTipoPoliza");
            _listaCatalogos.Add(23, "CatTipoRecup");
            _listaCatalogos.Add(24, "CatTipoRecuperacion");
            _listaCatalogos.Add(25, "CatVehiculoAsegurado");
            _listaCatalogos.Add(26, "CatEstatusH");
            _listaCatalogos.Add(27, "CatCircunstancia");            
            _listaCatalogos.Add(30, "CatTipoReporte");
            _listaCatalogos.Add(31, "CatSucursal");
            _listaCatalogos.Add(32, "CatModificacionIntegracion");
            _listaCatalogos.Add(33, "CatTipoMateria");
            _listaCatalogos.Add(34, "CatNotificadosEnCalidad");
            _listaCatalogos.Add(35, "CatTipoJuicio");
            _listaCatalogos.Add(36, "CatRamoBusqueda");
            _listaCatalogos.Add(37, "CatUbicacion");
            _listaCatalogos.Add(38, "CatRol");
            _listaCatalogos.Add(39, "CatRecuperacionEstatus");
            _listaCatalogos.Add(40, "CatRecuperacionTerceroInvolucrado");
            _listaCatalogos.Add(41, "CatRecuperacionTipoRecuperacion");
            _listaCatalogos.Add(42, "CatRecuperacionCausa");
            _listaCatalogos.Add(43, "CatRecuperacionTipoOficios");
            _listaCatalogos.Add(44, "CatRecuperacionAreaInvolucrada");
            _listaCatalogos.Add(45, "CatConducefConcluidoPendiente");
            _listaCatalogos.Add(46, "CatConducefArea");
            _listaCatalogos.Add(47, "CatConducefClave");
            _listaCatalogos.Add(48, "CatConducefDepartamentoDos");
            _listaCatalogos.Add(49, "CatConducefDepartamentoUno");
            _listaCatalogos.Add(50, "CatConducefDependencia");
            _listaCatalogos.Add(51, "CatConducefDescripcionMedioRecepcion");
            _listaCatalogos.Add(52, "CatConducefExpedienteProviene");
            _listaCatalogos.Add(53, "CatConducefNaturalezaActor");
            _listaCatalogos.Add(54, "CatConducefTipoMoneda");
            _listaCatalogos.Add(55, "CatConducefProductoServicio");
            _listaCatalogos.Add(56, "CatConducefRamo");
            _listaCatalogos.Add(57, "CatConducefEntidad");
            _listaCatalogos.Add(58, "CatConducefCausa");
            _listaCatalogos.Add(59, "CatAbogadoInterno");
            _listaCatalogos.Add(60, "CatFraudesMultiplesViasSolucion");
            _listaCatalogos.Add(61, "CatFraudesAreasQueIdentifica");
            _listaCatalogos.Add(62, "CatFraudesTipoDocumento");
            _listaCatalogos.Add(63, "CatFraudesProcedimientoEnContraDe");
            _listaCatalogos.Add(64, "CatSiabRespuesta");
            _listaCatalogos.Add(65, "CatSiabEstatus");
        }

        public List<Dictionary<string, object>> ObtenerCatalogo(ObtenerCatalogoInput input)
        {
            if (!_listaCatalogos.ContainsKey(input.Id))
                throw new UserFriendlyException(400, "No existe un catalogo con ese Id");

            var mTabla = "";
            _listaCatalogos.TryGetValue(input.Id, out mTabla);            

            string texto = string.Format("Select * from {0}", mTabla);

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);

            var result = new List<Dictionary<string, object>>();

            var reader = mCommand.ExecuteReader();
            
            while (reader.Read())
            {
                var mDic = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var mLlave = reader.GetName(i);
                    var mValor = reader[i];

                    //if (input.Id == 37 && mLlave == "menu")
                    //    mValor = JsonConvert.DeserializeObject<Object>(mValor.ToString());

                    mDic.Add(mLlave, mValor);

                }

                result.Add(mDic);

            }
            reader.Close();

            return result;
        }

        public ListarCatalogosOutput ListarCatalogos()
        {
            var result = new ListarCatalogosOutput();

            foreach (var i in _listaCatalogos)
            {
                result.ListaCatalogos.Add(new ListarCatalogosOutput.CatalogosDto
                {
                    Id = i.Key,
                    Nombre = i.Value
                });
            }

            return result;


        }

        public void AgregarRegistroCatalogo(AgregarRegistroCatalogoInput input)
        {
            if (!_listaCatalogos.ContainsKey(input.Id))
                throw new UserFriendlyException(400, "No existe un catalogo con ese Id");

            var mTabla = "";
            _listaCatalogos.TryGetValue(input.Id, out mTabla);

            string texto = $"INSERT INTO {mTabla}(Nombre) VALUES ('{input.Nombre}')";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);

            mCommand.ExecuteScalar();

            
        }

        public void ActualizarRegistroCatalogo(ActualizarRegistroCatalogoInput input)
        {
            if (!_listaCatalogos.ContainsKey(input.Id))
                throw new UserFriendlyException(400, "No existe un catalogo con ese Id");

            var mTabla = "";
            _listaCatalogos.TryGetValue(input.Id, out mTabla);

            string texto = $"UPDATE {mTabla} SET Nombre = '{input.Nombre}' WHERE Id = {input.IdRegistro}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);

            mCommand.ExecuteScalar();
        }

        public void EliminarRegistroCatalogo(EliminarRegistroCatalogoInput input)
        {
            if (!_listaCatalogos.ContainsKey(input.Id))
                throw new UserFriendlyException(400, "No existe un catalogo con ese Id");

            var mTabla = "";
            _listaCatalogos.TryGetValue(input.Id, out mTabla);

            string texto = $"DELETE FROM {mTabla} WHERE Id = {input.IdRegistro}";

            EnsureConnectionOpen();

            var mCommand = CreateCommand(texto, CommandType.Text);

            mCommand.ExecuteScalar();
        }

    }
}
