var vue_recharge = new Vue({
    el: "#vue_recharge",
    data: {

    },
    methods: {
        async UpdateActive(id) {
            const url = `/admin/recharge/change-active/${id}`;
            axios.get(url)
                .then(function (response) {
                    if (response.data.code === 200) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'Approved Success',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.reload();
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: `${response.data.message}`,
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                return;

                            }
                        });
                    }
                })
                .catch(function (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: `${response.data.message}`,
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            return;

                        }
                    });
                });

        },
        async getItemsByIdDelete(id) {

            try {
                Swal.fire({
                    title: 'Delete deposit application',
                    text: 'Are you sure you want to delete',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No!!!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#preloader').fadeIn();

                        const formData = new FormData();
                        formData.append('id', id);

                        axios.post('/admin/remove-recharge', formData, {
                            headers: {
                                'Content-Type': 'application/x-www-form-urlencoded'
                            }
                        }).then(response => {
                            if (response.data.code == 200) {
                                Swal.fire({
                                    icon: 'success',
                                    title: 'Success',
                                    text: 'Deleted successfully',
                                    confirmButtonText: 'OK'
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        window.location.reload();
                                    }
                                });
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error',
                                    text: `${response.data.message}`,
                                    confirmButtonText: 'OK'
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        return;

                                    }
                                });
                            }

                        }).catch(error => {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: 'An error occurred, please try again',
                                confirmButtonText: 'OK'
                            });
                        }).finally(() => {
                            $('#preloader').fadeOut();
                        });
                    } else {
                        return;
                    }
                });

            }
            catch (error) {
                $('#preloader').fadeOut();

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred, please try again',
                    confirmButtonText: 'OK'
                });
                return;

            }
        },
    }
})