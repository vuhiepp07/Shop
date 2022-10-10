//Conver product price from 1234567 to 1,234,567 
function normalizePrice(price){
    for(var i = price.length -1, x = 0 ; i> 0; i--, x++){
        if(x == 2){
            price = price.slice(0, i) + "," + price.slice(i)
            i++;
            x=-2;
        }
    }
    return price;
}

//display toast messages depend on input parameters
function toast({title = '', message = '', type = 'info', duration = 3000}){
    let toastList = document.getElementById('toast');
    if(toastList){
        const icons = {
            success : "fa-solid fa-circle-check",
            info :  "fa-solid fa-circle-info", 
            warning : "fa-solid fa-circle-exclamation",
            error : "fa-solid fa-triangle-exclamation"
        }
        var toastNode = document.createElement('div')

        let toastRemoveId = setTimeout(function(){
            toastList.removeChild(toastNode)
        }, duration + 1000)

        toastNode.onclick = function(e){
            if(e.target.closest('.toast__close')){
                toastList.removeChild(toastNode)
                clearTimeout(toastRemoveId)
            }
        }

        toastNode.classList.add('toast', `toast--${type}`)
        toastNode.style.animation = `toastSlideIn ease 0.3s, fadeOut linear 1s ${duration/1000}s forwards`
        toastNode.innerHTML = `
            <div class="toast__icon">
                <i class="${icons[type]}"></i>
            </div>
            <div class="toast__body">
                <h3 class="toast__title">
                    ${title}
                </h3>
                <p class="toast__msg">
                    ${message}
                </p>
            </div>
            <div class="toast__close">
                <i class="fa-solid fa-x"></i>
            </div>
        `
        toastList.appendChild(toastNode)             
    }
}

function MakeUserLogin(){
    toast({
        title: 'Thất bại',
        message: 'Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng',
        type: 'error',
        duration: 3000
    })
    // alert("Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng")
    var loginModal = document.querySelector('.login__modal')
    loginModal.classList.add('modal-active')
}

//Post obj to server and return to the frontend server respone
function SendHttpPostRequest(url, obj){
    let request = new XMLHttpRequest()
    let responeMessage = ''
    request.open("POST", url, false)
    request.setRequestHeader("Content-type", "application/json")
    request.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {
            responeMessage = JSON.parse(this.responseText)['status']
        }
    }
    request.send(JSON.stringify(obj));
    return responeMessage
}

function GenerateHTMLForProductItem(productList, i){
    let productPrice = String(productList[i]['price'])
    let productPriceInString = normalizePrice(productPrice)
    let productDiscountPrice = String(productList[i]['discountPrice'])
    let productDiscountPriceInString = normalizePrice(productDiscountPrice)

    let newHtml = `
        <div class="grid__column-TwoOfTen productList__item">
            <div class="productList__item-img" style="background-image: url(/images/products/${productList[i]['imageUrl']});">
                <div class="productList__item-addToCartbtn">
                    <input class="hiddenProductId" type="hidden" value=${productList[i]['productId']}>
                    Add to cart
                </div>
            </div>
            <div class="productList__item-name">
                <span>${productList[i]['productName']}</span>
            </div>
            
            <div class="productList__item-Price">
    `
    if(productPrice != productDiscountPrice && productDiscountPrice != 0){
        newHtml += `
        <div class="productList__item-Newprice">
            <span>${productDiscountPriceInString}₫</span>
        </div>
        <div class="productList__item-Oldprice">
            <span>${productPriceInString}₫</span>
        </div>
        `
    let productDiscountPercentage = ((productList[i]['price'] - productList[i]['discountPrice'])/productList[i]['price'])*100
    newHtml += `
        <div class="productList__item-SaleOf">
            <span style="line-height: 20px;" >GIẢM</span><br>
            <span>${productDiscountPercentage}%</span>
        </div>
    `
    }
    else{
        newHtml += `
            <div class="productList__item-Newprice">
                <span>${productPriceInString}₫</span>
            </div>
        `
    }
    
    newHtml += `</div>`
    newHtml += `</div>`
    return newHtml
}

function QueryProductItems_AndAddClickListener_ForThem(){
    let productElements = document.querySelectorAll('.productList__item');
    for(let i = 0; i < productElements.length; i++){
        productElements[i].addEventListener('click', function(e){
            let productId = productElements[i].querySelector('.hiddenProductId').value;
            window.location.href = `/home/productdetail/${productId}`;
        })
    }
}





