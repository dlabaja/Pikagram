const likedSrc = "/assets/icons/like-filled.png";
const unlikedSrc = "/assets/icons/like.png";

function onToggleLikePost(elemId, id) {
    const elem = document.getElementById(elemId);
    // lajknuto
    if (elem.value === "true") {
        fetch(`/api/posts/${id}/unlike`, {method: "post"})
        elem.getElementsByTagName("img")[0].src = unlikedSrc;
        elem.value = "false"
    } 
    else {
        fetch(`/api/posts/${id}/like`, {method: "post"})
        elem.getElementsByTagName("img")[0].src = likedSrc;
        elem.value = "true"
    }
}

function onToggleLikeComment(elemId, id) {
    const elem = document.getElementById(elemId);
    // lajknuto
    if (elem.value === "true") {
        fetch(`/api/comments/${id}/unlike`, {method: "post"})
        elem.getElementsByTagName("img")[0].src = unlikedSrc;
        elem.value = "false"
    }
    else {
        fetch(`/api/comments/${id}/like`, {method: "post"})
        elem.getElementsByTagName("img")[0].src = likedSrc;
        elem.value = "true"
    }
}

function postAndRefresh(url) {
    fetch(url, {method: "post"}).then(() => {
        window.location.reload();
    })
}