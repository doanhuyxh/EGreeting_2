var vue_email = new Vue({
    el: "#vue_email",
    data: {
        DataList: []
    },
    methods: {
         async GetDetails(id) {
            try {
                $('#preloader').fadeIn();
                const formData = new FormData();
                formData.append('id', id);
                await axios.post('/admin/detail-list-email', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) => {
                        $('#preloader').fadeOut();
                        if (response.data.code == 200) {
                            this.DataList = response.data.data;
                            $('#emailListModal').modal('show');
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

                    }).finally(() => {
                        $('#preloader').fadeOut();
                    });
            } catch (error) {
                console.error(error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error has occurred',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            }
        },
        async getItemsByIdDelete(id) {

            try {
                Swal.fire({
                    title: 'Delete emails',
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

                        axios.post('/admin/remove-email', formData, {
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
});