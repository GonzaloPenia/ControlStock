using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using dominio;
using System.Data.SqlClient;
using System.CodeDom;
using System.Net;

namespace negocio
{ 
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("Select A.Id, Codigo, Nombre, A.Descripcion as Descripcion, M.Descripcion as Marca, C.Descripcion as Categoria, ImagenUrl, Precio, M.Id as IdMarca, C.Id as IdCategoria From ARTICULOS A, CATEGORIAS C, MARCAS M Where M.Id = A.IdMarca And C.Id = A.IdCategoria");
                datos.EjecutarLectura();

                while (datos.Lector.Read()) 
                { 
                    Articulo aux = new Articulo();
                    aux.Identificador = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca= new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Categoria= new Categoria();
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];

                    if(!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Precio = Decimal.ToInt32((decimal)datos.Lector["Precio"]);

                    
                    lista.Add(aux);
                }
                return lista;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally { datos.CerrarConexion(); }
        }

        public void Agregar(Articulo nuevoArt)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("Insert into ARTICULOS (Codigo,Nombre,Descripcion,Precio,IdMarca,IdCategoria,ImagenUrl)values('"+nuevoArt.Codigo+"','"+nuevoArt.Nombre+"','"+nuevoArt.Descripcion+"',"+nuevoArt.Precio+",@IdMarca,@IdCategoria,@ImagenUrl)");
                datos.SetearParametro("@IdMarca",nuevoArt.Marca.Id);
                datos.SetearParametro("@IdCategoria",nuevoArt.Categoria.Id);
                datos.SetearParametro("@ImagenUrl",nuevoArt.ImagenUrl);
                
                datos.EjecutarAccion();
            }
            catch (Exception ex) { throw ex; }

            finally { datos.CerrarConexion(); }

        }

        public void Modificar(Articulo nuevoArt) 
        { 
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, ImagenUrl = @imagen, IdMarca = @idmarca, IdCategoria = @idcategoria, Precio = @precio Where Id = @id");
                datos.SetearParametro("@codigo", nuevoArt.Codigo);
                datos.SetearParametro("@nombre", nuevoArt.Nombre);
                datos.SetearParametro("@descripcion", nuevoArt.Descripcion);
                datos.SetearParametro("@imagen", nuevoArt.ImagenUrl);
                datos.SetearParametro("@idmarca", nuevoArt.Marca.Id);
                datos.SetearParametro("@idcategoria", nuevoArt.Categoria.Id);
                datos.SetearParametro("@precio", nuevoArt.Precio);
                datos.SetearParametro("@id", nuevoArt.Identificador);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("Delete From ARTICULOS Where id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Articulo> Filtrar(string campo, string criterio, string filtro)
        {
            {
                List<Articulo> lista = new List<Articulo>();
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion as Descripcion, M.Descripcion as Marca, C.Descripcion as Categoria, ImagenUrl, Precio, M.Id as IdMarca, C.Id as IdCategoria From ARTICULOS A, CATEGORIAS C, MARCAS M Where M.Id = A.IdMarca And C.Id = A.IdCategoria And ";
                    
                    if (campo == "Precio")
                    {
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Precio > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "Precio < " + filtro;
                                break;
                            default:
                                consulta += "Precio = " + filtro;
                                break;
                        }
                    }
                    else if (campo == "Nombre")
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Nombre like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "Nombre like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "Nombre like '%" + filtro + "%'";
                                break;
                        }
                    }
                    else
                    {   
                        //Por DESCRIPCION
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "A.Descripcion like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "A.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "A.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                    }

                    datos.SetearConsulta(consulta);
                    datos.EjecutarLectura();
                    while (datos.Lector.Read())
                    {
                        Articulo aux = new Articulo();
                        aux.Identificador = (int)datos.Lector["Id"];
                        aux.Codigo = (string)datos.Lector["Codigo"];
                        aux.Precio = Decimal.ToInt32((decimal)datos.Lector["Precio"]);
                        aux.Nombre = (string)datos.Lector["Nombre"];
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                        if (!(datos.Lector["ImagenUrl"] is DBNull))
                            aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                        aux.Marca = new Marca();
                        aux.Marca.Id = (int)datos.Lector["IdMarca"];
                        aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                        aux.Categoria = new Categoria();
                        aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                        aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex){throw ex;}
            }
        }
    }
}
