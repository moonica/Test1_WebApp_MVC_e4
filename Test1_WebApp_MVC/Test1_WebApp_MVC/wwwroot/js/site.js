'use strict';

//TODO: use document.ready

function confirmDelete(userId, path) {
    if (confirm(`Are you sure you want to delete user ${userId}?`)) {
        location.href = path;
    }
}
