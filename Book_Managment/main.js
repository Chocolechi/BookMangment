// variable definition
const url = 'https://localhost:44356/api/Books'





const container = document.querySelector('tbody')

let result = ''

const modalArticulo = new bootstrap.Modal(document.getElementById('modalArticulo'))
const formArticulo = document.querySelector('form')

const bookname = document.getElementById('bookname')
const description = document.getElementById('description')
var option = ''

btnCrear.addEventListener('click', () => {
    bookname.value = ''
    description.value = ''
    modalArticulo.show()
    option = 'crear'
})

// show result
const mostrar = (books) => {
    books.forEach(book => {
        result += `<tr>
                        <td>${book.id}</td>
                        <td>${book.name}</td>
                        <td>${book.description}</td>

                            <td class="text-center">
                            <a class="btnEdit btn btn-primary"> Edit </a> <a class="btnDelete btn btn-danger"> Delete </a>
                        </td>

                    </tr>   
                `
    });
    container.innerHTML = result

}

fetch(url)
    .then(response => response.json())
    .then(data => mostrar(data))


const on = (element, event, selector, handler) => {
    element.addEventListener(event, e => {
        if (e.target.closest(selector)) {
            handler(e)
        }
    })
}

on(document, 'click', '.btnDelete', e => {
    const fila = e.target.parentNode.parentNode
    const id = fila.firstElementChild.innerHTML
    alertify.confirm("This is a confirm dialog.",
        function () {
            fetch(url + '/' + id, {
                    method: 'DeleteBook',
                    mode: 'no-cors',
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Content-Type": "text/plain"
                    },
                })
                .then(res => res.json())
                .then(() => location.reload())
            // alertify.success('Ok')
        },
        function () {
            alertify.error('Cancel')
        })
})


let idForm = 0
on(document, 'click', '.btnEdit', e => {
    const fila = e.target.parentNode.parentNode
    idForm = fila.children[0].innerHTML
    const formName = fila.children[1].innerHTML
    const formDesc = fila.children[2].innerHTML
    bookname.value = formName
    description.value = formDesc
    option = 'editar'
    modalArticulo.show()
})

formArticulo.addEventListener('submit', (e) => {
    e.preventDefault()
    if (option == 'crear') {
        fetch(url, {
                method: 'CreateBook',
                mode:'no-cors',
                headers: {
                    'Content-Type': 'application/json',
                    "Access-Control-Allow-Origin": "*",
                    "Content-Type": "text/plain"
                },
                body: JSON.stringify({
                    bookname: bookname.value,
                    description: description.value
                })
            })
            .then(response => response.json())
            .then(data => {
                const newArticle = []
                newArticle.push(data)
                mostrar(newArticle)
            })

    }
    if (option == 'editar') {
        fetch(url + '/' + idForm,{
            method:'UpdateBook',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                bookname: bookname.value,
                description: description.value
            })   
        })
        .then(response => response.json())
        .then(response => location.reload())
    }
    modalArticulo.hide()
})