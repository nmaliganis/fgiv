import { createStore } from "vuex";
import { config } from "vuex-module-decorators";

import AuthModule from "@/store/modules/AuthModule";
import BodyModule from "@/store/modules/BodyModule";
import ConfigModule from "@/store/modules/ConfigModule";
import ThemeModeModule from "@/store/modules/ThemeModeModule";

config.rawError = true;

const store = createStore({
  modules: {
    AuthModule,
    BodyModule,
    ConfigModule,
    ThemeModeModule,
  },
});

export default store;
