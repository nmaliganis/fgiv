import { createRouter, createWebHashHistory } from "vue-router";

import Home from "./views/home-page";
import Tasks from "./views/suspects-page.vue";
import defaultLayout from "./layouts/side-nav-inner-toolbar";
import auth from "@/auth";


const router = new createRouter({
  routes: [
    {
      path: "/home",
      name: "home",
      meta: {
        requiresAuth: false,
        layout: defaultLayout
      },
      component: Home
    },
    {
      path: "/suspects",
      name: "suspects",
      meta: {
        requiresAuth: false,
        layout: defaultLayout
      },
      component: Tasks
    },
    {
      path: "/",
      redirect: "/suspects"
    },
    {
      path: "/recovery",
      redirect: "/suspects"
    },
    {
      path: "/:pathMatch(.*)*",
      redirect: "/suspects"
    }
  ],
  history: createWebHashHistory()
});

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (!auth.loggedIn()) {
      next({
        name: "home",
        query: { redirect: to.fullPath }
      });
    } else {
      next();
    }
  } else {
    next();
  }
});

export default router;
