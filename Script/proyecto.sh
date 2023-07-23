#!/bin/bash

run() {
    # Código para ejecutar el proyecto
    echo "Ejecutando el proyecto..."
    dotnet watch run --project MoogleServer
}

compile_report() {
    # Código para compilar y generar el informe en PDF
    echo "Compilando el informe..."
    # Agrega aquí los comandos para compilar y generar el PDF del informe LaTeX
}

compile_slides() {
    # Código para compilar y generar las diapositivas en PDF
    echo "Compilando las diapositivas..."
    # Agrega aquí los comandos para compilar y generar el PDF de las diapositivas LaTeX
}

show_report() {
    # Verificar si el archivo PDF del informe existe
    if [ ! -f "informe.pdf" ]; then
        compile_report
    fi

    # Código para mostrar el informe en un visor de PDFs
    echo "Mostrando el informe..."
    # Agrega aquí el comando para abrir el PDF del informe en un visor
}

show_slides() {
    # Verificar si el archivo PDF de las diapositivas existe
    if [ ! -f "diapositivas.pdf" ]; then
        compile_slides
    fi

    # Código para mostrar las diapositivas en un visor de PDFs
    echo "Mostrando las diapositivas..."
    # Agrega aquí el comando para abrir el PDF de las diapositivas en un visor
}

clean() {
    # Código para eliminar los archivos auxiliares
    echo "Limpiando archivos auxiliares..."
    # Agrega aquí los comandos para eliminar los archivos auxiliares generados
    
    # Por ejemplo, si los archivos auxiliares tienen extensiones específicas:
    find . -type f ( -iname "*.aux" -o -iname "*.log" -o -iname "*.toc" ) -delete
}

case "$1" in
    run)
        run
        ;;
    report)
        compile_report
        ;;
    slides)
        compile_slides
        ;;
    show_report)
        show_report
        ;;
    show_slides)
        show_slides
        ;;
    clean)
        clean
        ;;
    *)
        echo "Uso: proyecto.sh [opción]"
        echo "Opciones disponibles:"
        echo "run           Ejecutar el proyecto"
        echo "report        Compilar y generar el PDF del informe"
        echo "slides        Compilar y generar el PDF de las diapositivas"
        echo "show_report   Mostrar el informe en un visor de PDFs"
        echo "show_slides   Mostrar las diapositivas en un visor de PDFs"
        echo "clean         Eliminar archivos auxiliares"
        ;;
esac