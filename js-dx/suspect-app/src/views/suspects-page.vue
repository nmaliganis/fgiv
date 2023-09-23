<template>
  <div>
    <h2 class="content-block">Suspects</h2>

    <dx-data-grid
      class="dx-card wide-card"
      :data-source="suspectsItems"
      :focused-row-index="0"
      :show-borders="false"
      :focused-row-enabled="true"
      :column-auto-width="true"
      :column-hiding-enabled="true"
    >
      <dx-paging :page-size="10" />
      <dx-pager :show-page-size-selector="true" :show-info="true" />
      <dx-filter-row :visible="true" />

      <dx-column data-field="Is" caption="Suspect ID" :width="90" :hiding-priority="2" />

      <dx-column
        data-field="Firstname"
        caption="Firstname Suspect"
        :width="190"
      />

      <dx-column
        data-field="Lastname"
        caption="Lastname Suspect"
      />

      <dx-column
        data-field="Calls"
        caption="Calls"
      />

      <dx-column
        data-field="Title"
        caption="Title"
        :allow-sorting="true"
      />

      <dx-column
        data-field="Nationality"
        caption="Nationality"
      />

      <dx-column
        data-field="Dob"
        caption="Dob"
      />

      <dx-column
          data-field="Location.Street"
          caption="Street"
      />

      <dx-column
          data-field="Location.Postcode"
          caption="Postcode"
      />

      <dx-column
        data-field="Location.City"
        caption="City"
      />

      <dx-column
          data-field="Location.Country"
          caption="Country"
      />
    </dx-data-grid>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import "devextreme/data/odata/store";
import DxDataGrid, {
  DxColumn,
  DxFilterRow,
  DxPager,
  DxPaging
} from "devextreme-vue/data-grid";

const priorities = [
  { name: "High", value: 4 },
  { name: "Urgent", value: 3 },
  { name: "Normal", value: 2 },
  { name: "Low", value: 1 }
];

export default {
  setup() {
    return {
      priorities
    };
  },
  computed: {
    ...mapGetters(["isLoadingSuspects", "isLoadSuspects", "suspectsItems", "isLoadSuspect", "suspect", "messageError"])
  },
  methods: {
    ...mapActions(["getSuspectsItems"]),
  },
  mounted() {
    this.getSuspectsItems();
  },
  components: {
    DxDataGrid,
    DxColumn,
    DxFilterRow,
    DxPager,
    DxPaging
  }
};
</script>
