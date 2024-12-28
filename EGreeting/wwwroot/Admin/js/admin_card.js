var vue_card = new Vue({
    el: "#vue_card",
    data: {
        CategoryData: [],
        IDCard: 0,
        NameCard: "",
        Slug: "",
        ImageCardPreview: "",
        ImageCardMain: "",
        CategoryID: "",
        ShortDecription: "",
        TypeJsonCard: [],

        imageFile: null,
        previewImage: null,
        uploadedImage: null,

        coordinates: "",
        fontList: []
    },
    mounted() {
        this.fetchDataCategory();

        // lấy dữ liệu từ url query id
        var url = new URL(window.location.href);
        this.IDCard = url.searchParams.get("id");
        if (this.IDCard != null) {
            this.fetchDataCard();
        }

        fetch("/admin/font-list")
            .then(res => res.json())
            .then(res => {
                const fontArray = Object.entries(res).map(([font, font_path]) => ({ font, font_path }));
                this.fontList = fontArray;
            })

    },
    methods: {
        fetchDataCategory() {
            axios.get("/admin/category-list")
                .then((response) => {
                    this.CategoryData = response.data;
                    return Promise.resolve();
                });
        },
        fetchDataCard() {
            axios.get(`/admin/get-card/${this.IDCard}`)
                .then((response) => {
                    response.data = response.data.data;
                    this.NameCard = response.data.nameCard;
                    this.Slug = response.data.slug;
                    this.ImageCardPreview = response.data.imageCardPreview;
                    this.previewImage = response.data.imageCardPreview;
                    this.ImageCardMain = response.data.imageCardMain;
                    this.CategoryID = response.data.categoryID;
                    this.ShortDecription = response.data.shortDecription;
                    if (response.data.typeJsonCard != null) {

                        this.TypeJsonCard = JSON.parse(response.data.typeJsonCard);
                    }
                });
        },
        onFileChange(event) {
            this.imageFile = event.target.files[0];            
            this.ImageCardMain = URL.createObjectURL(this.imageFile);
        },
        onFileChange2(event) {
            this.imageFile = event.target.files[0];
            this.previewImage = URL.createObjectURL(this.imageFile);
        },

        async AddCard() {
            try {
                $('#preloader').fadeIn();

                const formData = new FormData();
                formData.append('IDCard', this.IDCard);
                formData.append('NameCard', this.NameCard);
                formData.append('ShortDecription', this.ShortDecription);
                formData.append('CategoryID', this.CategoryID);
                formData.append('TypeJsonCard', JSON.stringify(this.TypeJsonCard));

                if (this.$refs.PrPath.files[0] != null) {

                    formData.append('ImageCardMainPath', this.$refs.PrPath.files[0]);
                }
                if (this.$refs.PrPath2.files[0] != null) {

                    formData.append('ImageCardPreviewPath', this.$refs.PrPath2.files[0]);
                }

                await axios.post('/admin/save-card', formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then((response) => {
                        $('#preloader').fadeOut();
                        if (response.data.code == 200) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Thành công',
                                text: 'Đã lưu thành công',
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    window.location.href = "/admin/card"
                                }
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Lỗi',
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
                    title: 'Lỗi',
                    text: 'Đã có lỗi xảy ra',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        return;

                    }
                });
            }
        },
        AddFile() {
            this.TypeJsonCard.push({
                "name": "",
                "point": "0,0",
                "color": "#000000",
                "size": 12,
                "font_family": "",
                max_width: 100
            })
        },
        RemoveFile(index) {
            this.TypeJsonCard.splice(index, 1);
        },
        GetPoint(event) {
            const rect = event.target.getBoundingClientRect();
            const x = ((event.offsetX / event.target.clientWidth) * event.target.naturalWidth).toFixed(0);
            const y = ((event.offsetY / event.target.clientHeight) * event.target.naturalHeight).toFixed(0);

            this.coordinates = `Tọa độ trên ảnh: (x: ${Math.floor(x)}, y: ${Math.floor(y)})`;
        }
    }
})