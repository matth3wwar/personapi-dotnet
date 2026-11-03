#!/usr/bin/env bash
# Pruebas de la API REST de personapi-dotnet usando curl.
#
# Uso (en Git Bash o WSL, con la app corriendo):
#   ./pruebas-api.sh                         -> Visual Studio (http://localhost:5108)
#   ./pruebas-api.sh http://localhost:8080   -> Docker
#
# Crea datos de prueba, los consulta, edita y borra. Al final no deja nada en la base.

URL=${1:-http://localhost:5108}
CC="9900001"        # identificación de la persona de prueba
TEL="3009900001"    # número de teléfono de prueba
CUERPO=""           # cuerpo de la última respuesta
OK=0
FALLOS=0

# En WSL, "localhost" es Linux y no llega a la app de Windows.
# Por eso se usa el curl.exe de Windows, que sí la alcanza.
CURL=curl
if grep -qi microsoft /proc/version 2>/dev/null && command -v curl.exe >/dev/null; then
    CURL=curl.exe
fi

# probar "descripción" MÉTODO ruta código_esperado [json]
probar() {
    local desc=$1 metodo=$2 ruta=$3 esperado=$4 json=$5
    local respuesta codigo

    # La respuesta trae el cuerpo y, en la última línea, el código HTTP
    if [ -n "$json" ]; then
        respuesta=$($CURL -s -w "\n%{http_code}" -X "$metodo" "$URL$ruta" \
            -H "Content-Type: application/json" -d "$json")
    else
        respuesta=$($CURL -s -w "\n%{http_code}" -X "$metodo" "$URL$ruta")
    fi
    codigo=${respuesta##*$'\n'}
    CUERPO=${respuesta%$'\n'*}

    if [ "$codigo" = "$esperado" ]; then
        echo "[OK]    $desc"
        OK=$((OK + 1))
    else
        echo "[FALLO] $desc (esperado $esperado)"
        FALLOS=$((FALLOS + 1))
    fi
    echo "        $metodo $ruta -> $codigo"
    # Muestra la respuesta recortada para que no llene la terminal
    [ -n "$CUERPO" ] && echo "        ${CUERPO:0:200}"
    echo
}

echo "Probando la API en $URL (usando $CURL)"
echo

# Comprobar que la app está corriendo
if ! $CURL -s "$URL/swagger/v1/swagger.json" > /dev/null; then
    echo "No se pudo conectar a $URL. ¿Está corriendo la app?"
    exit 1
fi

# Limpieza por si quedaron datos de una ejecución anterior
$CURL -s -X DELETE "$URL/api/personas/$CC" > /dev/null

echo "=== Swagger ==="
probar "Documento OpenAPI disponible" GET "/swagger/v1/swagger.json" 200
probar "Interfaz Swagger UI disponible" GET "/swagger/index.html" 200

echo "=== Profesiones ==="
probar "Crear profesión" POST "/api/profesiones" 201 \
    '{"nom":"Ingeniero de prueba","des":"Creada por pruebas-api.sh"}'
# Toma el id generado por la base de datos
ID_PROF=$(echo "$CUERPO" | grep -o '"id":[0-9]*' | head -1 | cut -d: -f2)
echo "        (id de la profesión creada: $ID_PROF)"
echo
probar "Listar profesiones" GET "/api/profesiones" 200
probar "Crear profesión sin nombre (inválida)" POST "/api/profesiones" 400 '{"des":"sin nombre"}'

echo "=== Personas ==="
probar "Crear persona" POST "/api/personas" 201 \
    "{\"cc\":\"$CC\",\"nombre\":\"Ana\",\"apellido\":\"Prueba\",\"genero\":\"F\",\"edad\":25}"
probar "Crear persona repetida" POST "/api/personas" 409 \
    "{\"cc\":\"$CC\",\"nombre\":\"Ana\",\"apellido\":\"Prueba\",\"genero\":\"F\",\"edad\":25}"
probar "Crear persona con letras en la identificación" POST "/api/personas" 400 \
    '{"cc":"abc","nombre":"Ana","apellido":"Prueba","genero":"F","edad":25}'
probar "Crear persona con género inválido" POST "/api/personas" 400 \
    '{"cc":"9900002","nombre":"Ana","apellido":"Prueba","genero":"X","edad":25}'
probar "Consultar persona" GET "/api/personas/$CC" 200
probar "Consultar persona que no existe" GET "/api/personas/0000000" 404
probar "Editar persona" PUT "/api/personas/$CC" 204 \
    "{\"cc\":\"$CC\",\"nombre\":\"Ana Maria\",\"apellido\":\"Prueba\",\"genero\":\"F\",\"edad\":26}"
probar "Editar con identificación distinta a la ruta" PUT "/api/personas/$CC" 400 \
    '{"cc":"123","nombre":"Ana","apellido":"Prueba","genero":"F","edad":26}'
probar "Ver persona editada" GET "/api/personas/$CC" 200

echo "=== Teléfonos ==="
probar "Crear teléfono" POST "/api/telefonos" 201 \
    "{\"num\":\"$TEL\",\"oper\":\"Claro\",\"dueno\":\"$CC\"}"
probar "Crear teléfono de una persona que no existe" POST "/api/telefonos" 400 \
    '{"num":"3001112233","oper":"Claro","dueno":"0000000"}'
probar "Editar teléfono" PUT "/api/telefonos/$TEL" 204 \
    "{\"num\":\"$TEL\",\"oper\":\"Movistar\",\"dueno\":\"$CC\"}"
probar "Consultar teléfono" GET "/api/telefonos/$TEL" 200

echo "=== Estudios ==="
probar "Crear estudio" POST "/api/estudios" 201 \
    "{\"idProf\":$ID_PROF,\"ccPer\":\"$CC\",\"fecha\":\"2020-06-15\",\"univer\":\"Universidad Nacional\"}"
probar "Crear estudio repetido" POST "/api/estudios" 409 \
    "{\"idProf\":$ID_PROF,\"ccPer\":\"$CC\",\"fecha\":\"2020-06-15\",\"univer\":\"Universidad Nacional\"}"
probar "Editar estudio" PUT "/api/estudios/$ID_PROF/$CC" 204 \
    "{\"idProf\":$ID_PROF,\"ccPer\":\"$CC\",\"fecha\":\"2021-12-01\",\"univer\":\"Javeriana\"}"
probar "Consultar estudio" GET "/api/estudios/$ID_PROF/$CC" 200

echo "=== Borrado en cascada ==="
probar "Borrar persona (también sus teléfonos y estudios)" DELETE "/api/personas/$CC" 204
probar "El teléfono ya no existe" GET "/api/telefonos/$TEL" 404
probar "El estudio ya no existe" GET "/api/estudios/$ID_PROF/$CC" 404
probar "Borrar profesión" DELETE "/api/profesiones/$ID_PROF" 204
probar "Borrar profesión que ya no existe" DELETE "/api/profesiones/$ID_PROF" 404

echo "Resultado: $OK correctas, $FALLOS fallidas"
[ "$FALLOS" -eq 0 ]
